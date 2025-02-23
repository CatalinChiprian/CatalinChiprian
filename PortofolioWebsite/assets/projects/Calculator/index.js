const display = document.getElementById("display");

const body = document.querySelector("body");

body.addEventListener("keydown", function(event) {
    switch (event.key) {
        case "Enter":
            calculate();
            break;
        case "Escape":
            clearDisplay();
            break;
        case "Backspace":
            if (display.value != "Error")
            display.value = display.value.slice(0, -1);
            else
            clearDisplay();
            break;
        case ",":
            if(IsValidInput("."))
                appendToDisplay(".");
            break;
        case ".":
            if(IsValidInput("."))
                appendToDisplay(".");
            break;
        case "+":
            if(IsValidInput("+"))
                appendToDisplay("+");
            break;
        case "-":
            if(IsValidInput("-"))
                appendToDisplay("-");
            break;
        case "*":
            if(IsValidInput("*")) {
                appendToDisplay("*");
            }
            break;
        case "/":
            if(IsValidInput("/")) {
                appendToDisplay("/");
            }
            break;
        default:
            if (event.key >= 0 && event.key <= 9) {
                appendToDisplay(event.key);
            }
            break;
    }
});

function IsValidInput(input) {
    if (input == "*" && display.value[display.value.length-1] == "/")
        return false;
    else if (input == "/" && display.value[display.value.length-1] == "*")
        return false;
    else if (input == ".") {
        for (let i = display.value.length; i >= 0; i--) {
            if (display.value[i] == "+" || display.value[i] == "-" || display.value[i] == "*" || display.value[i] == "/")
                break;
            else if (display.value[i] == ".")
                return false;
        }
        return true;
    }
    else if 
    ((display.value == "Error") || 
    (display.value == "") ||
    (display.value[display.value.length-1] == input)) {
        return false;
    }

    return true;
}

function appendToDisplay(input) {
    if (input == "+" || input == "-" || input == "*" || input == "/" || input == ".") {
        if (IsValidInput(input))
            display.value += input;
    }
    else {
        display.value += input;
    }
}

function clearDisplay() {
    display.value = "";
}

function calculate() {
    try {
        display.value = eval(display.value);
    }
    catch (error)
    {
        display.value = "Error";
    }
}