   M S B A M L   `   `   `  ����L  HTicketReservation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TicketReservationTicketReservation  5   	   "    TicketReservation.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:TicketReservation     5   	   -   5          � Title$#  Evidence rezervací vstupenek��5   	   $	��550��6.   $	��700��6;   $	��550��6G   $	��700��6W    � 	Resources 5	      7   1TicketReservation.TicketKindEnumToStringConverter   KindStringConverterG   ATicketReservation.TextBoxLengthToHelpTextBlockVisibilityConverter  HelperVisibilityConverterG   ATicketReservation.ReservationFileInfoToStringInformationConverter  LoadedFilesInfoConverter?   9TicketReservation.SectionInfoToStringInformationConverter  LoadedSectionsInfoConverterB   <TicketReservation.RoomLayoutInfoToStringInformationConverter  LoadedRoomsInfoConverter��%�   &	        &	      &	 &     &	 9     &	 L       5
   
   60     5   
   6@     5   
   6@     5   
   68     5   
   6;   5       J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ ApplicationCommands.New��5      6Z     5   
   -   5        $ ApplicationCommands.Open��5      6[     5   
   -   5        $ ApplicationCommands.Save��5      6[     5   
   -   5        $! local:CustomCommands.SaveAs��5      6^     5   
   -   5        $ local:CustomCommands.Exit��5      6\     5   
   -   5        $ ApplicationCommands.Help��5      6[     5   
   -   5        $  local:CustomCommands.About��5      6]   .��5      ]� .+�5   
   o� $	��Top=�6   $	��5,0q�6$   .@�5      m� $��Soubor��6   .@�5      m� $��Nový��6    m� Command$ ApplicationCommands.New��6)   m� 5      $��	Otevřít��6   $ ApplicationCommands.Open��6,   m� 5       $��Uložit��6   $ ApplicationCommands.Save��6+   m� 5!      $��Uložit jako...��6   $! local:CustomCommands.SaveAs��63   �� 5"      m� 5#      $��Zavřít��6   $ local:CustomCommands.Exit��6+   5$      m� 5%      $
��Info��6   .@�5&      m� $��
Nápověda��6   $ ApplicationCommands.Help��6-   m� 5'      $��
O programu��6   $  local:CustomCommands.About��6/   5(      5)      9� 5+   
   -	   5        $	��Top=�5+      $��5q�6(   $	��8,2q�63   $+��%Výchozí hodnoty pro nové rezervace��6A   $	��Top=�6m   $��1q�6�   ���	#FF858585.��5,      �  5-       �� 
TargetType  MarginStyle �� Property �� Value �� BasedOn��%{  &	       '�� �     7  '�� �     7  '�� �     7  '�� �     �� 5.      " ��6   .�5/      ��  � ��$ 	10,5,25,5q�63   �� 50       � ��$ Center=�6>   �� 51       � ��$ 2q�64   52      �� 54      8   " ��6C   �� 55      8   " ��6C   �� 56      8   " ��6C   �� 58      " ��6   .�59      ��  � ��$ Center=�6>   5:      5;      .+�5=      �� 	 �� Orientation$	 
Horizontal=�6   .+�5>      �� $��Vložil:��6    � 5?      .+�5@      �� -
   5        $��DefaultUserTextBox��5@   .   $	��150��6"   6.   �� 5A      
 �� Padding$

 15,0q�6$   $"��Výchozí operátor aplikace��63   ���	#FF949494$��Italic$�6k    J� IsHitTestVisible .  $
��Left=�5B   $   y�6?   ��  �� Path$ Text.Length� �� ElementName$ DefaultUserTextBox�� �� 	Converter# [ 5C      5D      �� 5F      $	 
Horizontal=�6   .+�5G      �� $��Zapsáno dne:��6    �� 5H      -   5        $��DefaultBookedOnTextBox��5H   *   $	��150��6   6*   5I      �� 5K      $	 
Horizontal=�6   .+�5L      �� $��
Zaplaceno:��6    �� 5M      -   5        $��DefaultSoldCheckBox��5M      5N      �� 5P      $	 
Horizontal=�6   .+�5Q      �� $��Druh:��6    �� 5R      -   5        $��DefaultKindComboBox��5R   +   $	��150��6   ��5S      �� 5T      .R�5U   "   �� ��6,   �� # [  5V      5X      5Y      5Z      5[      �� 5]   
   $��Bottom=�6   $��5q�6,   $��©Štof, 2017 - 2018��67   �� 5_   
   -   5        $��MainTabControl��5_   "   $	��5,0q�6   .@�5`      �� -   5        $��ManagerTabItem��5`      $��Souborový manažer��6,   .��5a      �  � ColumnDefinitions 5b      �� 5c      $��* �6+   �� 5d      $��* �6+   �� 5e      $��* �6+    � RowDefinitions 5g      �� 5h      $
��Auto �6(   �� 5i      $
��Auto �6(   �� 5j      $
��Auto �6(   �� 5k      $��* �6(   �� 5l      $
��Auto �6(   .+�5o      � $��3��6    .+�5p      �� -   5        $��Přidat sekci��5p   !   $��5q�68   6C   5q      �� 5s      $��1��6    $��3��6-   �� 5u      $��Načtené soubory:��6    $��	Underline{�68   $��2��6T   $��0��6a   $��Center=�6q   $��5q�6�   �� 5v      $��Načtené sekce:��6    $��	Underline{�66   $��2��6R   $��1��6_   $��Center=�6o   $��5q�6�   �� 5w      $��Načtené sály:��6    $��	Underline{�65   $��2��6Q   $��2��6^   $��Center=�6n   $��5q�6�   �� 5y      -   5        $��LoadedFilesListBox��5y   _   $��3��6   $��0��6+   $	��5,0q�6y    �� SelectionMode$ Single=�6�   $��0,0,10,0q�6�   ��6;   �� *LoadedFiles+��5z      �� 5{      .R�5|   "   �  5}   &   �� 5~   *   $
��Auto �6;   �� 5   *   $
��Auto �6;   �� 5�   *   $��* �6;   .+�5�   &   �� -   5        $��Center=�5�   -   $��Center=�6J   $��X��6e   $��Lucida)�6q   $��10(�6�   $��	ExtraBold"�6�   ��6�   ��  �� RelativeSource �� *Self+$ ActualHeight�6�   	� 5�   &   -   5        $��3q�5�   2    	� 	GroupName$ LoadedFilesRBGroup��6=   $��1��6y   �� 5�   &   $��2��6F   ��60   �� *Name+6F   5�   #   5�      5�      �� 5�      -   5        $��LoadedSectionsListBox��5�   b   $��3��6   $��1��6+   $	��5,0q�6   $ Single=�6�   $��0,0,10,0q�6�   ��6;   �� *LoadedSections+��5�      �� 5�      .R�5�   "   �  5�   &   �� 5�   *   $
��Auto �6;   �� 5�   *   $��* �6;   .+�5�   &   �� $��3q�6/    d� 	IsChecked$
 TrueY�6:   �� 5�   &   $��1��6M   ��60   �� *SectionName+6M   5�   #   5�      5�      �� 5�      -   5        $��LoadedLayoutsListBox��5�   ;   $��3��6   $��2��6+   $	��5,0q�6W   $ Single=�6d   $��0,0,10,0q�6�   ��5�      �� 5�      .R�5�   "   �  5�   &   �� 5�   *   $
��Auto �6;   �� 5�   *   $��* �6;   .+�5�   &   �� $��3q�6/   $
 TrueY�6:   �� 5�   &   $��1��6J   ��60   �� *
RoomName+6J   5�   #   5�      5�      �� 5�      $��4��6    $��0��6-   $��5q�6�   $��40��6�   $��WrapWithOverflow=�6�   ��6=   �� $ LoadedFilesListBox��$ SelectedItem�# [ 6�   �� 5�      $��4��6    $��1��6-   $��5q�6�   $��40��6�   $��WrapWithOverflow=�6�   ��6=   �� $ LoadedSectionsListBox��$ SelectedItem�# [ 6�   �� 5�      $��4��6    $��2��6-   $��5q�6�   $��40��6�   $��WrapWithOverflow=�6�   ��6=   �� $ LoadedLayoutsListBox��$ SelectedItem�# [ 6�   5�      5�      5�      5�      5�      