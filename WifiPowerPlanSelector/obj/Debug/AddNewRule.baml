   M S B A M L   `   `   `  ����  WifiPowerPlanSelector&     WifiPowerPlanSelector.AddNewRule   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Add new WiFi Rule��5   	   $	��260��6#   $	��280��60    � 
ResizeMode$ CanMinimize=�6<    � WindowStyle$
 None=�6U   
���   �
�������$��1q�6�    � WindowStartupLocation$ CenterOwner=�6�    � AllowsTransparency . .��5      � .+�5   
   �� -   5         ��Name$ CloseButton��5      $��r��6&   $��Right=�62   $	��Top=�6N   $��35��6f   $��30��6�   $��16(�6�    � ToolTip$ Cancel��6�   $��Webdings)�6�   $��0q�6�   ��6q   Z� ��5      �� 5       �� 
TargetType" ��6   .�5	      ��  �� Property � ��	 �� Value	 63   Z� �� 5
       � ��
	 �����63   �� 5       � ��	 5      �� 5      
 �� 
TargetType"
 ��6.   .R�5   "   �� #��z��6)   ._�5   &   �� $��Center=�67   $��Center=�6T   5   #   5      5       �� Triggers 5      P� 5       P� Property �@J�IsMouseOver P� Value . . �5      ��  � ��	 �	#FFFF54546;   �� 5       � ��
	 �����6;   5      5      5      �� 5   
   $
��Left=�6   $��	10,22,0,0q�6/   $
��Wrap=�6B   $��Add new WiFi rule��6V   $	��Top=�6o   $��24(�6�   �� 5   
   -   5         ��Name$ wifiComboBox��5      $
��Left=�6)   $��
10,101,0,0q�6D   $	��Top=�6X   $	��258��6p    �� DisplayMemberPath$
 SSID��6|    �� SelectedValuePath$
 SSID��6�    �� SelectedValue 6�   ��  �� Path$
 WiFi��� 5   
   $��Select WiFi:��6   $
��Left=�6'   $��	10,70,0,0q�6B   $	��Top=�6U   �� 5    
   -   5        $ powerPlanComboBox��5       $
��Left=�6.   $��
10,159,0,0q�6I   $	��Top=�6]   $	��258��6u   $
 Name��6�   �� 5!   
   $��Select power plan:��6   $
��Left=�6-   $��
10,128,0,0q�6H   $	��Top=�6\   �� 5"   
   -   5        $ SaveRuleButton��5"      $��Save new rule��6)   $
��Left=�6A   $��
10,199,0,0q�6\   $	��Top=�6p   $	��106��6�   $��38��6�   6�   5#       � Triggers 5$      :� 5%   
    :� RoutedEvent$ Window.Loaded��6   .[�5&      �� .��5'      �� .��5(      W�  g� Duration$ 0:0:.2D�6&    �� TargetProperty$ Opacity�68    W� From$ 0X�6\   	 W� To$ 1X�6e    g� AccelerationRatio$ .1Q�6l   5)      5*      5+      5-      