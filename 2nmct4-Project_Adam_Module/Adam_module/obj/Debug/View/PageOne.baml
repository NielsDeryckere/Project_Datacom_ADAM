   M S B A M L   `   `   `  ����F  BAdam_module, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:Adam_module.ViewModelAdam_module.ViewModel  5      3clr-namespace:Adam_module.ViewAdam_module.View  5      )clr-namespace:Adam_moduleAdam_module  5          Adam_module.View.PageOne   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2vm#clr-namespace:Adam_module.ViewModel     5      ,vclr-namespace:Adam_module.View     5      'cclr-namespace:Adam_module     5        � DataContext  5      %  Adam_module.ViewModel.PageOneVM  5   
    � 	Resources 5      '  !Adam_module.StateToColorConverter   StateToColorConverter  TemplateDemo �� 
TargetType �� 	Converter  TemplateDemo2 �� Mode  TemplateDemo3  TemplateDemo4  TemplateDemo5��%r  &	        &	      7   &	 �     7   &	 B    7   &	 �    7   &	 �    7     5   
   6"   �� 5   
   " ��6/   .R�5      � .+�5      A� $��50��6i   $��50��6t   ��6   �� *
Devices[0]+8   6t   5      5      �� 5   
   " ��60   .R�5      � .+�5      A� $
��auto��6u   $
��auto��6�   ��6   �� *
Devices[1]+$ TwoWay=�8   6�   5!      5"      �� 5#   
   " ��60   .R�5$      � .+�5%      A� $
��auto��6u   $
��auto��6�   ��6   �� *
Devices[2]+$ TwoWay=�8   6�   5'      5(      �� 5*   
   " ��60   .R�5+      � .+�5,      A� $
��auto��6u   $
��auto��6�   ��6   �� *
Devices[3]+$ TwoWay=�8   6�   5.      5/      �� 50   
   " ��60   .R�51      � .+�52      A� $
��auto��6u   $
��auto��6�   ��6   �� *
Devices[4]+$ TwoWay=�8   6�   54      55      57      .��59      � 
������� � RowDefinitions 5;   
   �� 5<      $��* �6   �� 5=      $��* �6   �� 5>      $��* �6    � ColumnDefinitions 5@   
   �� 5A      $��* �6   �� 5B      $��3* �6   �� 5C      $��* �6   .+�5E   
   �� $��10q�6   $��3��6"   $��2��63   .@�5H      �� $	��Map��6   .��5K      � $��0q�6    5L      �� 5M      $��* �6(   �� 5N      $��* �6(   �� 5O      $��* �6(    5Q      �� 5R      $��* �6+   �� 5S      $��* �6+   �� 5T      $��* �6+   .+�5W      �� $����C:\Users\niels\Google Drive\Semester 4\Datacommunication\Project_Datacom_ADAM\2nmct4-Project_Adam_Module\Adam_module\bin\Debug\HuisVisualCrop.jpg��6   $��3��6�   $��3��6�   �� 5Y      -   5        $��btnVentilator��5Y      #��[ 6~   $��76,117,98,33q�6�    J� RenderTransformOrigin$ 
-0.9,-1.08&�6�   $��1��6�   $��1��6�   ��63   �� *CommandConnect+��6V   �� *
Buttons[0]+6�   �� 5Z      -   5        $��	btnBureau��5Z      #��[ 6{   $��100,161,111,26q�6�   ��6/   �� *CommandConnect+��6S   �� *
Buttons[1]+6�   �� 5[      -   5        $��btnZitruimte��5[      #��[ 6   $��100,84,111,103q�6�   $��1��6�   ��63   �� *CommandConnect+��6W   �� *
Buttons[2]+6�   �� 5\      -   5        $��	btnKeuken��5\      #��[ 6z   $��19,170,186,10q�6�   $��2��6�   $��1��6�   ��6.   �� *CommandConnect+��6R   �� *
Buttons[3]+6�   �� 5]      -   5        $��	btnGarage��5]      #��[ 6z   $��121,21,94,169q�6�   $ 
1.74,-5.12&�6�   $��2��6�   $��1��6�   ��6.   �� *CommandConnect+��6R   �� *
Buttons[4]+6�   5e      5f      �� 5g      $��Settings��6   .��5h      �  5i      �� 5j      $��* �6(   �� 5k      $��* �6(   �� 5l      $��* �6(    5n      �� 5o      $��* �6+   �� 5p      $��* �6+   .+�5r      �� $��Add card��6   $��0��61   $��0��6>   $��10q�6O   {�6[   �� *Admin+.��5s      �  5t      �� 5u   "   $��* �60   �� 5v   "   $��* �60   �� 5w   "   $��* �60    5y      �� 5z   "   $��* �63   �� 5{   "   $��* �63   .+�5}      �� $��5q�6F   $��1��6Q   $��0��6^   ��6'   �� *Roles+ �� SelectedItem 6n   �� *SelectedItem+�� 5~      $��Selecteer rol��6$   $��5q�6<   �� 5      $��Voeg toe��6%   $��5q�68   $��1��6C   $��1��6S   ��6`   �� *NewCardCommand+5�      5�      �� 5�      $��Reset kaart��6   $��1��64   $��0��6A   $��10q�6Q   {�6]   �� *Admin+.��5�      �  5�      �� 5�   "   $��* �60   �� 5�   "   $��* �60   �� 5�   "   $��* �60    5�      �� 5�   "   $��* �63   �� 5�   "   $��* �63   .+�5�      �� $��Reset kaart��6%   $��10q�6;   $��1��6G   $��0��6T   5�      5�      5�      5�      5�      �� 5�   
   $��Afmelden��6   $
��Left=�6$   $��5q�6?   $��Bottom=�6J   $��75��6e   $��30��6�   $��2��6�   $��1��6�   ��6p   �� *LogoutCommand+6�   �� 5�   
   $��Connection ADAM:��6   $
��Left=�6+   $��5,50,10,162q�6F   $��2��6\   �� 5�   
   $��Logged in as: ��6   $
��Left=�6)   $��5,5,0,0q�6D   $	��Top=�6U   $��30��6m   $��2��6y   $	��100��6�   $��1��6�   �� 5�   
   $
��Left=�6   $��10,40,0,186q�6/   $	��100��6b   $��30��6n   $��2��6z   $��1��6�   ��6D   �� *LoggedInUser+6�   �� 5�   
   $��0��61   $��10,99,5,123q�6>   $��2��6S   ��6   �� *	Succeeded+6S   �� 5�   
   -   5        $��
btnConnect��5�      $
��Left=�6#   $��	Reconnect��6>   $��10,151,0,64q�6u   $��2��6�   ��6R   �� *CommandConnect+6�   5�      5�      