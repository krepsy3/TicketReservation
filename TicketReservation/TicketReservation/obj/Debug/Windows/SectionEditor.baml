   M S B A M L   `   `   `  ����L  HTicketReservation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TicketReservationTicketReservation  5      %    TicketReservation.SectionEditor   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:TicketReservation     5      -   5        $	��450��5        � 	Resources  5	      A   ;TicketReservation.SelectionStateEnumToEditingStateConverter   StateConverterG   ATicketReservation.SelectionStateEnumToEditingButtonStateConverter  StateButtonConverter8   2TicketReservation.SelectedKindIndexToBoolConverter  KindIndexConverter1   +TicketReservation.SoldBoolToStringConverter  SoldStringConverter2   ,TicketReservation.SeatNoIntToStringConverter  SeatStringConverter7   1TicketReservation.TicketKindEnumToStringConverter  KindStringConverter  BoolToVisConverter<   6TicketReservation.InvertedBooleanToVisibilityConverter  InvBoolToVisConverterG   ATicketReservation.TextBoxLengthToHelpTextBlockVisibilityConverter  HelperVisibilityConverter��%  &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r     &	 �     &	 �       5
   
   6:     5   
   6@     5   
   61     5   
   6*     5   
   6+     5   
   60   �� 5   
   6'     5   
   65     5   
   6@   5       J� CommandBindings 5      )	  #System.Windows.Input.CommandBinding	  5   
   -   5         	  Command$  ApplicationCommands.Delete��5      6[   .��5      �� .@�5   
   �� $��Tabulka rezervací��6   .��5      � -   5        $��MainGrid��5       � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $
��Auto �6'   .+�5!      �� -   5        $��MainListView��5!   P   $��5q�6   -
  'System.Windows.Input.KeyboardNavigation 
  TabNavigation$ Continue=�6d   ��6&   ��  �� Path$ Reservations�  5"       �� 
TargetType �� Property �� Value	 �� Padding 	 ItemContextMenu
 m� Command��%4  '��       &		 �     �� 5#      " ��6    .�5$      ��  � ��$
 Left=�6K   �� 5%       � �� 5&   "   �� 5'   &   .R�5(   *   �� $		 4,0q�6E   ��64   �� 6E   5)   '   5+      5,      �� 5.      .@�5/      m� $��Smazat��6'   $ 
 ApplicationCommands.Delete��67   50      51      ��52      �� 53      " ��6    .�54      ��  �@�ContextMenu# [	 6<   55       �� View 58      �� 59       �� AllowsColumnReorder . .P�5:      �� $��Jméno��6-    �� DisplayMemberBinding 6<   �� *Name+�� 5;      $
��Druh��6-    6;   �� *Kind+ �� 	Converter# [ �� 5<      $��
Místo č.��6-    6?   �� *SeatNo+# [ �� 5=      $��Kód vstupenky��6-    6D   �� *
TicketCode+�� 5>      $��	Zaplaceno��6-    6@   �� *Sold+# [ �� 5?      $��Zadal��6-    6<   �� *User+�� 5@      $��Zadáno dne��6-    6A   �� *
BookedOn+ �� StringFormat$ {0:d. M. yyyy}���� 5A      $��Kontakt��6-    6>   �� *	Contact+5B      5D      �� 5F      -   5        $��1��5F      $
��Auto=�6/   .��5G      �  � RowDefinitions 5H      �� 5I      $
��Auto �6,   �� 5J      $
��Auto �6,   .+�5M      9� -   5        $��ValueEditor��5M   0   $��0��6#   $��1q�6C   ���#858585$��5q�6m   $	��8,3q�6x   $	��Top=�6�    9� 
IsExpanded . ��5O      �� 5P   "   .+�5Q   &   �� $��Podrobnosti��60   y�6C   �� $ 
IsExpanded� �� RelativeSource ��  �� AncestorType$ ExpanderL� �� Mode$ FindAncestor=�# [ �� 5R   &   -   5        $��StateTextBlock��5R   _   $��0��60   $��2��6=   $��13(�6Q   $��Stretch=�6u   y�5S   )   �� $ 
IsExpanded� �� $ ExpanderL�$ FindAncestor=�# [ ��5T   )   �� $ CurrentSelectionState�# [  $ Režim: {0}��5U   #   .��5X      � $	��250��6#   $��-8q�6/    5Y   "   �� 5Z   &   $
��Auto �64   �� 5[   &   $
��Auto �64   �� 5\   &   $
��Auto �64   �� 5]   &   $
��Auto �64   �� 5^   &   $
��Auto �64   �� 5_   &   $
��Auto �64   �� 5`   &   $
��Auto �64   �� 5a   &   $
��Auto �64   �� 5b   &   $
��Auto �64   �� 5c   &   $
��Auto �64    5e   "   �� 5f   &   $
��Auto �67   �� 5g   &   $��* �67     5i   "    
 MarginStyle �� Triggers P� Property P� Value �� BasedOn��%_  &	
       '�� [    7 
 '�� �    7 
 '� �    7 
 '��     7 
 '�� �    7 
 '�� �    7 
 '�� �    7 
 �� 5j   &   " �6,    5k   *   P� 5l   .    � ��$ 0��6M   . �5m   2   ��  � ��$ 10,5,5,5q�6K   �� 5n   2    � ��$ Right=�6X   5o   /   P� 5q   .    � ��$ 1��6M   . �5r   2   ��  � ��$ 0,5,10,5q�6K   �� 5s   2    � ��$ Stretch=�6X   5t   /   .�5v   *   ��  � ��$ Center=�6N   5w   '   �� 5y   &   8   " ��6S   .�5z   *   ��  � ��$ 2q�6D   5{   '   �� 5}   &   8   " ��6S   .�5~   *   ��  � ��$	 7,2q�6D   5   '   �� 5�   &   8   " �6S    5�   *   P� 5�   .    � ��$ 0��6M   . �5�   2   ��  � ��$ Center=�6X   5�   /   5�   '   �� 5�   &   8   " ��6S   �� 5�   &   8   " ��6S   �� 5�   &   8   " ��6S   �� 5�   &   8   " ��6S   5�   #   .+�5�   "   �� $��0��6,   $��Jméno:��69   �� 5�   "   -   5        $��EditorNameTextBox��5�   G   $��0��6*   $��1��67   6`   �� 5�   "   $��0��6,   $��1��69   $		 5,0q�6I   $��Jméno rezervujícího��6W   ���	#FF949494$��Italic$�6�    J� IsHitTestVisible .  $
��Left=�5�   E   y�6`   �� $ Text.Length� �� ElementName$ EditorNameTextBox��# [ �� 5�   "   $��1��6,   $��Kontakt:��69   �� 5�   "   -	   5        $��EditorContactTextBox��5�   G   $��1��6*   $��1��67   6c   �� 5�   "   $��1��6,   $��1��69   $		 5,0q�6I   $ ��Kontakt na rezervujícího��6W   ���	#FF949494$��Italic$�6�    .  $
��Left=�5�   E   y�6`   �� $ Text.Length�$ EditorContactTextBox��# [ �� 5�   "   $��2��6,   $��Druh:��69   �� 5�   "   -
   5        $��EditorKindComboBox��5�   H   $��2��6+   $��1��68   ��5�   &   �� 5�   *   .R�5�   .   �� ��68   �� # [ 5�   +   5�   #   �� 5�   "   $��3��6,   $��Číslo stolu:��69   �� 5�   "   -   5        $��EditorSeatNoTextBox��5�   �   $��3��6*   $��1��67   {�6G   �� $ EditorKindComboBox��$ SelectedIndex�# [ 6�   �� 5�   "   $��3��6,   $��1��69   $		 5,0q�6I   $��Číslo místa k sezení��6W   ���	#FF949494$��Italic$�6�    .  $
��Left=�5�   E   y�6`   �� $ Text.Length�$ EditorSeatNoTextBox��# [ �� 5�   "   $��4��6,   $��Kód vstupenky:��69   �� 5�   "   -   5        $��EditorTicketCodeTextBox��5�   G   $��4��6*   $��1��67   6f   �� 5�   "   $��4��6,   $��1��69   $		 5,0q�6I   $$��Unikátní textové označení��6W   ���	#FF949494$��Italic$�6�    .  $
��Left=�5�   E   y�6`   �� $ Text.Length�$ EditorTicketCodeTextBox��# [ �� 5�   "   $��5��6,   $��
Zaplaceno:��69   �� 5�   "   -   5        $��EditorSoldCheckBox��5�   H   $��5��6+   $��1��68   6b   �� 5�   "   $��6��6,   $��Vložil:��69   �� 5�   "   -   5        $��EditorUserTextBox��5�   G   $��6��6*   $��1��67   6`   �� 5�   "   $��6��6,   $��1��69   $		 5,0q�6I   $��Operátor aplikace��6W   ���	#FF949494$��Italic$�6�    .  $
��Left=�5�   E   y�6`   �� $ Text.Length�$ EditorUserTextBox��# [ �� 5�   "   $��7��6,   $��Zapsáno dne:��69   �� 5�   "   -   5        $��EditorBookedOnTextBox��5�   G   $��7��6*   $��1��67   6d   �� 5�   "   $��7��6,   $��1��69   $		 5,0q�6I   $��Formát data 1.1.1970��6W   ���	#FF949494$��Italic$�6�    .  $
��Left=�5�   E   y�6`   �� $ Text.Length�$ EditorBookedOnTextBox��# [ � 5�   "   $��8��6,   $��2��69   .+�5�   &   �� -   5        $��ConfirmReservationButton��5�   �   $��0,5,5,5q�6-   ��6>   �� $ CurrentSelectionState�# [ 6�   �� 5�   &   -   5        $��5,5,0,5q�5�   -   $��Zrušit změny��6>   6U   5�   #   5�      5�      9� 5�      -   5        $��FilterPicker��5�   0   $��1��6#   $��1q�6D   ���#858585$��5q�6n   $	��8,3q�6y   $	��Top=�6�    .  $��Filtrování��5�   #   .��5�      �� $��COMING SOON��6(   $��10q�6;   5�      5�      5�      5�      5�      �� 5�   
   $��Rozvržení sálu��6   .��5�      �� $��Visible=�6   $��Visible=�6C   $��-2q�6i   .��5�      � $��Stretch=�6   $��Stretch=�63   $��6q�6Q   .+�5�      �� 
����İ� �� RadiusX$ 2��68    �� RadiusY$ 2��6D   $��-2q�6P   � 5�      -   5        $��RoomLayoutGrid��5�   U   $��Stretch=�6   $��Stretch=�67   6U   5�      5�      5�      5�      5�      