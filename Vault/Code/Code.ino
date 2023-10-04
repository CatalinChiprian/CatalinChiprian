mi#include <Servo.h>
#include <EEPROM.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <SPI.h>
#include <Wire.h>

#define OLED_RESET 4
#define OLED_WIDTH 128
#define OLED_HEIGHT 64

Adafruit_SSD1306 display(OLED_WIDTH, OLED_HEIGHT, &Wire, -1);

// st-cp pin 12
#define LATCH_PIN 10
// sh-cp pin 11
#define CLOCK_PIN 11
// ds pin 14
#define DATA_PIN 12

// rotery output pins 6 , 7
#define ROTERY_OUT_A 6
#define ROTERY_OUT_B 7
#define ROTERY_BUTTON 2

#define SERVO_PIN 5

// buttons
#define PASS_CONFIRM_BUTTON 13
#define LOCK_BUTTON 4
#define CHANGE_PASS_BUTTON 8

enum memory
{
  TIMER = 0,
  LOCKSTATE = 2,
  PASSWORD1 = 7,
  PASSWORD2 = 8,
  PASSWORD3 = 9,
  WRONGTRIES = 10,
  TIMEOUT = 20
};

// OLED
// Connect SCL to A5 and SDA to A4
int wrongTries = 0;
int timer = 60;
int timeOut = 0;

// for GUI
String serialData;
char command;

// var for rotary encoder
int rotaryCounter = 0;
int rotaryCurrentStateA;
int rotaryLastStateA;
String rotaryCurrentDir = "";
int chosenSeg = 0;
int password[3];
// other vars
Servo myServo;
int numbers[3] = {0, 0, 0};

unsigned long lastTimeButtonStateChanged = 0;
unsigned long lockButLastTimeButtonStateChanged = 0;
unsigned long submitButlastTimeButtonStateChanged = 0;
unsigned long changeButLastTimeButtonStateChanged = 0;
unsigned long lastChangeTime = 0;
unsigned long BuzzerDuration = 0;

// var for looping 7seg in general loop function
int i = 0;


void ChangeCode(int newCode[])
{
  EEPROM.update(PASSWORD1, newCode[0]);
  EEPROM.update(PASSWORD2, newCode[1]);
  EEPROM.update(PASSWORD3, newCode[2]);
  Serial.println("Code changed to - " + (String)newCode[0] + newCode[1] + newCode[2]);
}

byte IsLocked()
{
  return EEPROM.read(LOCKSTATE);
}

void GetPass(int password[])
{
  for (int i = 0; i < 3; i++)
  {
    password[i] = EEPROM.read(i + 7);
  }
}

String GetCurrentPass()
{
  String data;
  data += EEPROM.read(PASSWORD1);
  data += EEPROM.read(PASSWORD2);
  data += EEPROM.read(PASSWORD3);
  return data;
}

void NewCodeToArr(int code[], int number)
{

  for (int i = 2; i >= 0; i--)
  {
    code[i] = number % 10;
    number /= 10;
  }
}

// Timeout function that activates a lockdown when you input 3 wrong tries

void rememberTimer()
{
  timer = EEPROM.read(TIMER);
}

void rememberWrongTries()
{
  wrongTries = EEPROM.read(WRONGTRIES);
}

void rememberTimeOut()
{
  timeOut = EEPROM.read(TIMEOUT);
}


void sendDataToShift(int data)
{
  digitalWrite(LATCH_PIN, LOW);
  shiftOut(DATA_PIN, CLOCK_PIN, MSBFIRST, data);
  shiftOut(DATA_PIN, CLOCK_PIN, MSBFIRST, data >> 8);
  digitalWrite(LATCH_PIN, HIGH);
}

int BuzzerOut()
{
  unsigned long duration = millis();
  // play the buzzer when is buzzerduration is less than 5000
  if (BuzzerDuration > duration)
  {
    return 2;
  }
  else
  {
    return 0;
  }
}


void timeOutFunction()
{
  // This will be used to set a delay
  unsigned long previousTimeOutMillis = 0UL;
  unsigned long timeOutinterval = 1000UL;
  unsigned long currentTimeOutMillis = millis();
  while (timeOut == 1)
  {
    rememberTimer();
    // Set up the display and write the time left
    display.clearDisplay();
    display.setTextColor(WHITE);
    display.setTextSize(2);
    display.setCursor(10, 28);
    display.println("Timeout:");
    display.setCursor(103, 28);
    display.print(timer);
    // This code will be executed once every second
    unsigned long currentTimeOutMillis = millis();
    if (currentTimeOutMillis - previousTimeOutMillis > timeOutinterval)
    {
      timer--;
      previousTimeOutMillis = currentTimeOutMillis;
    }
    // Save timer to memory
    EEPROM.update(TIMER, timer);
    display.display();
    // Inform user that the timer has ended
    if (timer <= 0)
    {
      display.clearDisplay();
      display.setTextSize(2);
      display.setCursor(15, 18);
      display.print("Timeout");
      display.setCursor(15, 38);
      display.print("expired.");
      display.display();
      int period = 5000;
      unsigned long time_now = 0;
      time_now = millis();
      while (millis() < time_now + period)
      {
        // wait approx. 5 ms
      }
      timer = 60;  // reset timer
      timeOut = 0; // turn off the timeout
      EEPROM.update(TIMEOUT, timeOut);
      EEPROM.update(TIMER, timer); // reset the memory
    }
    // If there is no timeout this will display nothing
  }
  if (timeOut == 0)
  {

    display.clearDisplay();
    display.setTextColor(WHITE);
    display.setTextSize(2);
    display.setCursor(45, 28);
    display.println(" ");
    display.display();
  }
}

// this function shows the numbers in 7seg , on or off the LEDs and play the buzzer
void showData()
{
  // START shift registary build and output

  // empity 16bit for shift registery
  int myEmpity = 0B000000000000000;

  // set bits for the 7seg to turn on in order
  int first7Seg = 0B00010000;
  int second7Seg = 0B00100000;
  int third7Seg = 0B01000000;

  // array for 7seg bit on shift registery
  int my7Seg[3] = {first7Seg, second7Seg, third7Seg};

  // data on the first shift registery 7seg common and 4bit data to BCD
  int firstShift = my7Seg[i] | numbers[i];

  int secondShift = 0B00000000; // data on the second shift registery

  firstShift = firstShift << 8;

  int completeData = myEmpity | firstShift;

  if (IsLocked())
  {
    secondShift = secondShift | 0B00001000; // turn on the red led
  };
  if (!IsLocked())
  {
    secondShift = secondShift | 0B00000100; // turn on the green led
  };
  // turning on the dot on the 7seg
  switch (chosenSeg)
  {
  case 0:
    secondShift = secondShift | 0B01100000;
    break;
  case 1:
    secondShift = secondShift | 0B01010000;
    break;
  case 2:
    secondShift = secondShift | 0B00110000;
    break;
  }

  secondShift = secondShift | BuzzerOut(); // turn the buzzer on

  completeData = completeData | secondShift; // build complete data

  // push bits to shhift register for showing numbers on 7 sigments and leds and buzzer
  sendDataToShift(completeData);

  // restart the loop for 7seg
  i += 1;
  if (i == 3)
    i = 0;

  // END
}
void lock()
{
  myServo.write(90);
  EEPROM.update(LOCKSTATE, 1);
  numbers[0] = 0;
  numbers[1] = 0;
  numbers[2] = 0;
}

void unLock()
{
  GetPass(password);
  if (numbers[0] == password[0] && numbers[1] == password[1] && numbers[2] == password[2])
  {
    wrongTries = 0;
    EEPROM.update(10, wrongTries); // reset the wrong tries
    myServo.write(180);            // move the servo
    EEPROM.update(LOCKSTATE, 0);           // change the lock state
  }
  else // if the password is not correct
  {
    BuzzerDuration = millis() + 2000; // play the buzzer for next 2 second
    rememberWrongTries();             // remember from memory the wrong tries left
    wrongTries++;
    EEPROM.update(WRONGTRIES, wrongTries); // save the wrong tries in memory
    // Count the wrong tries, activate timeout and then reset the counter
    if (wrongTries >= 3)
    {
      timeOut = 1;                   // activate timeout
      EEPROM.update(TIMEOUT, timeOut);    // save timeout
      wrongTries = 0;                // reset wrong tries
      EEPROM.update(WRONGTRIES, wrongTries); // save it in the memory
    }
    if (timeOut == 1)
    {
      // Turn off everything when lockdown is active
      sendDataToShift(0);
      timeOutFunction();
    }
  }
}

void gui()
{
  if (Serial.available())
  {
    serialData = Serial.readString();
    command = serialData.charAt(0);

    switch (command)
    {
    case 'L':
      if (!IsLocked())
      {
        lock();
        Serial.println("lock");
      }
      break;
    case 'U':
      if (IsLocked())
      {
        // Change the locked state to false
        unLock();
        Serial.println("unlock");
      }
      break;

    case 'C':
      Serial.println("Current code - " + GetCurrentPass());
      break;
    case 'N':
      int number = serialData.substring(1).toInt();
      int code[3];
      NewCodeToArr(code, number);
      ChangeCode(code);
      break;
    case 'D':
      String text = serialData.substring(1);
      if (timer == 0)
      {
        Serial.println("Text dispalyed!");
        display.clearDisplay();
        display.setTextColor(WHITE);
        display.setTextSize(1);
        display.setCursor(10, 28);
        display.println(text);
        display.display();
      }
    }
  }
}

void rotery()
{
  // START rotery encoder

  // for perventing multi count & debounce
  unsigned long currentChangeTime = millis();

  // Read the current state of ROTERY_OUT_A
  rotaryCurrentStateA = digitalRead(ROTERY_OUT_A);
  // If last and current state of ROTERY_OUT_A are different, then pulse occurred
  // React to only 1 state change to avoid double count

  //  if (rotaryCurrentStateA != rotaryLastStateA  && rotaryCurrentStateA == 1 && currentChangeTime - lastChangeTime > 5)
  if (rotaryCurrentStateA != rotaryLastStateA && rotaryCurrentStateA == 1)
  {
    lastChangeTime = currentChangeTime;
    // If the ROTERY_OUT_B state is different than the ROTERY_OUT_A state then
    // the encoder is rotating CCW so decrement
    if (digitalRead(ROTERY_OUT_B) != rotaryCurrentStateA)
    {
      if (rotaryCounter < 9)
        rotaryCounter++;
      else
        rotaryCounter = 0;
      rotaryCurrentDir = "CCW";
    }
    else
    {
      // Encoder is rotating CW so increment
      if (rotaryCounter > 0)
        rotaryCounter--;
      else
        rotaryCounter = 9;
      rotaryCurrentDir = "ACW";
    }

    numbers[chosenSeg] = rotaryCounter;
  }
  // Remember last ROTERY_OUT_A state
  rotaryLastStateA = rotaryCurrentStateA;
  // END OF rotery

  // START of roterybutton

  // START of chosing which 7seg is on
  byte lastButtonState = LOW;
  unsigned long debounceDuration = 250; // millis

  if (millis() - lastTimeButtonStateChanged > debounceDuration)
  {
    byte buttonState = digitalRead(ROTERY_BUTTON);
    if (buttonState != lastButtonState)
    {
      lastTimeButtonStateChanged = millis();
      lastButtonState = buttonState;
      if (buttonState == HIGH)
      {
        if (chosenSeg < 2)
          chosenSeg++;
        else
          chosenSeg = 0;

        rotaryCounter = numbers[chosenSeg];
      }
    }
  }
}


void passSubmitButton()
{

  byte lastPassSubmitButtonState = LOW;
  unsigned long submitButDebounceDuration = 250; // millis

  if (millis() - submitButlastTimeButtonStateChanged > submitButDebounceDuration)
  {
    byte buttonState = digitalRead(PASS_CONFIRM_BUTTON);
    if (buttonState != lastPassSubmitButtonState)
    {
      submitButlastTimeButtonStateChanged = millis();
      lastPassSubmitButtonState = buttonState;
      if (buttonState == HIGH && IsLocked())
      {
        unLock();
      }
    }
  }
}

void lockButton()
{
  byte lockButLastPassSubmitButtonState = LOW;
  unsigned long lockButDebounceDuration = 200; // millis

  if (millis() - lockButLastTimeButtonStateChanged > lockButDebounceDuration)
  {
    byte buttonState = digitalRead(LOCK_BUTTON);
    if (buttonState != lockButLastPassSubmitButtonState)
    {
      lockButLastTimeButtonStateChanged = millis();
      lockButLastPassSubmitButtonState = buttonState;
      if (buttonState == HIGH)
      {
        if (!IsLocked())
          lock();
      }
      else if (buttonState == LOW)
      {
        // play the buzzer as long as the dore opend and it shouldn't
        //  BuzzerDuration = millis() + 1;
      }
    }
  }
}

void passChangeButton()
{
  byte changeButLastPassSubmitButtonState = LOW;
  unsigned long changeButDebounceDuration = 200; // millis

  if (millis() - changeButLastTimeButtonStateChanged > changeButDebounceDuration)
  {
    byte buttonState = digitalRead(CHANGE_PASS_BUTTON);
    if (buttonState != changeButLastPassSubmitButtonState)
    {
      changeButLastTimeButtonStateChanged = millis();
      changeButLastPassSubmitButtonState = buttonState;
      if (buttonState == HIGH && (!IsLocked()))
      {
        ChangeCode(numbers);
      }
    }
  }
}

void timeoutCheck()
{
  rememberTimeOut(); // First of all remember if there is a timeout active
  if (timeOut == 1)
  {
    // Update the Servo to Locked state
    EEPROM.update(LOCKSTATE, 1);
    myServo.write(90);
    // Turn off everything when lockdown is active
    sendDataToShift(0);
    timeOutFunction();
  }
}

void setup()
{

  // setup OLED screen
  display.begin(SSD1306_SWITCHCAPVCC, 0x3C);

  // set shift registery pins as output
  pinMode(LATCH_PIN, OUTPUT);
  pinMode(CLOCK_PIN, OUTPUT);
  pinMode(DATA_PIN, OUTPUT);

  // Set encoder pins as inputs
  pinMode(ROTERY_OUT_A, INPUT);
  pinMode(ROTERY_OUT_B, INPUT);
  pinMode(ROTERY_BUTTON, INPUT);

  myServo.attach(SERVO_PIN);

  Serial.begin(9600);

  // read the state of rotery encoder
  rotaryLastStateA = digitalRead(ROTERY_OUT_A);
}

void loop()
{

  timeoutCheck(); // Check for timeOut

  showData(); // Display data

  gui(); // read and do the command that comes from gui(serial)

  rotery(); // Input password

  passSubmitButton(); // Correct password? -> Unlock

  passChangeButton(); // Change password?

  lockButton(); // Lock?

}
