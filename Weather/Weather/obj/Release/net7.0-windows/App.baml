   M S B A M L   `   `   `  ����   Weather    Weather.App   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       �� 	Resources     ScrollBarTrackThumb �� 
TargetType �� Property �� Value �� 
TargetType �Name ��Name �� CornerRadius �� Triggers	 P� Property
 P� Value �� 
TargetName � RowDefinitions ]�Name ]� Thumb n�Name ]� IncreaseRepeatButton ��Name J� Opacity ]� DecreaseRepeatButton P� 
SourceName  ButtonSelectBrush  	DarkBrush �� Angle  CustomTextBoxStyle �� BasedOn ��Name  TextWatermark  TextBox  
TextButton �� Triggers }� Location }� 	PenOffset  Activebutton 
 Button��%m  &	        '�� �    &	 �    7��&	 �    &	 �    &	 b    &	 �	    &	 �
    �� " n�.���  � �� �� " n�.R�� $
 Grid��.+��� $��Stretch=�$��Stretch=�$
��Auto��$
��Auto��
������ �� $ CornerScrollBarRectangle��$ 5��$��Stretch=�$��Stretch=�$
��Auto��$
��Auto��$��0,1,0,1q�#��z�� P� 	 �@�Tag$
 
Horizontal��. ��� $ CornerScrollBarRectangle�� � ��$
 Auto���� $ CornerScrollBarRectangle�� � ��$ 6���� " ��.���  �@��IsFlicksEnabled$ false����  � �� �#ADABAB��  � ��
 ���� ��  � ��$ 7����  � �� �� " ��.R�� $ GridRoot��$��30��#��z�� �� $��0.00001* �.+�]� $ 
PART_Track��$��0��$
��true��$|�false�� n� $ Thumb��#��z��#���    �� $ PageUp��$��ScrollBar.PageDownCommand��$ 0Q�$|�false�� �� $ PageDown��$��ScrollBar.PageUpCommand��$ 0Q�$|�false�� P� $ Thumb��	 �@J�IsMouseOver$

 true��. ��� # �  $ Thumb�� � ��P� $ Thumb��	 �@n�
IsDragging$

 true��. ��� # �  $ Thumb�� � ��P� 	 � {�$
 false��. ��� $ Thumb�� � y�$ 	Collapsed=�P� 	 �@��Orientation$
 
Horizontal=�. ��� $ GridRoot�� �@�LayoutTransform �� $	 -90Q��� $ 
PART_Track�� �@�LayoutTransform �� $	 -90Q���  � ��$
 Auto����  � ��$ 8���� $ Thumb�� �@�Tag$ 
Horizontal���� $ PageDown�� � ��$ ScrollBar.PageLeftCommand���� $ PageUp�� � ��$  ScrollBar.PageRightCommand���� " ��8   .���  � �� �� " ��.R��� $ Bd��#��z�����	#FFE6DDDD#��z��$ 10��._��� $ PART_ContentHost�� P� 	 � {�$
 False��. ��� # � ��$ Bd�� � ���� # � �� � ��P� 	 � ��$

 Auto��. ���  � ��$	 100��P� 	 � ��$

 Auto��. ���  � ��$ 20���� " ��.���  � ��
 ���p���  � ��$ 14(���  � ��$ 1����  �@K�ZIndex$ 1����  � ��$ 10 0 0 0q���  � ��$ Center=��� " ��.���  � ��
 ���� ��  � ��
 �iii���  � ��$ 14(���  � ��$ 10 0 0 0q���  � ��$ 1����  � ��
 ���� ��  � ��$ 0q���  � ��$ Center=��� " ��.���  � �� �� " ��.R��� $��16(�$��SemiBold"����#333333�� $��Center=�$��Center=� P� 	 �@J�IsMouseOver$

 True��. ���  � �� �� " ��.R��� $��16(�$��SemiBold"����#333333��}� $ 	Underline=�$ 3Q��� $��Center=�$��Center=��� " ��.���  � ��
 �   ���  � ��
 �������  � ��$ 0q���  � ��$ 38����  � ��$ 38����  � ��$
 Bold"���  � �� �� " ��.R��� #��z��$ 50��._��� $��Center=�$��Center=� P� 	 �@J�IsMouseOver$

 True��. ���  � ��
 �   ���  � ��
 ������� " ��.���  � ��
 �������  � ��
 �   ���  � ��$ 0q���  � ��$ 38����  � ��$ 38����  � ��$
 Bold"���  � �� �� " ��.R��� #��z��$ 50��._��� $��Center=�$��Center=� P� 	 �@J�IsMouseOver$

 True��. ���  � ��
 �   ���  � ��
 �����