   M S B A M L   `   `   `  ����L  HTicketReservation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TicketReservationTicketReservation  5   	   *    $TicketReservation.OptionPickerDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:TicketReservation     5   	   -   5        $	��300��5   	     � WindowStartupLocation$  CenterOwner=�6    � WindowStyle$ 
ToolWindow=�69    J� CommandBindings 5	      )  #System.Windows.Input.CommandBinding  5
   
   -   5           Command$ MediaCommands.Select��5
      6O   .��5      ]� .+�5   
   �� -   5        $��messageTextBlock��5      $	��Top=�6,   $��10q�6A   $
��Wrap=�6M   �� 5   
   $��Bottom=�6    �� Orientation$ 
Horizontal=�6-   $��RightToLeft=�6F   .+�5      �� $��OK��6   $��10q�6"   $��MediaCommands.Select��6.   $��50��6M   �� 5      -   5        $��Cancel��5      $��10q�6&   $��50��6?   5      �� 5   
   -   5        $��radiosStackPanel��5      5      5      