   M S B A M L   `   `   `  ����C  ?Project3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Project3Project3  5   	       Project3.Room   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Project3     5   	     � Title$
  Room��5   	   $	��450��6   $	��800��6#    � 	Resources 5
       �� 
TargetType �� Property �� Value��%-  '��       �� 5   
   " ��6   .�5      ��  � ��$ Segoe UI)�6+   �� 5       � ��$ 14(�6)   �� 5       � z� 5      �� 5      5      �� 5       � �� �#5B3F296,   �� 5       � ��$ 2q�60   �� 5       � �� �#5B3F296+   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��233* �6   �� 5      $
��186* �6   ��5   
   �� 5       �� ImageSource$K EC:\Users\FlukeAndBright\source\repos\Project3\Project3\Images\bg3.png��6    j� Stretch$ UniformToFill=�6m   .+�5!   
   �� -   5         ��Name$ 	name_game��5!      
���   �$��	ExtraBold"�66   $��Italic$�6M   $��Rondalo)�6`   $��33(�6u   $��
Room List ��6�   $
��Left=�6�   $��	58,31,0,0q�6�   $	��Top=�6�   $	��212��6�   $��51��6�   �� 5"   
   -   5        $��Rondalo)�5"      $
��Join��6&   
��� ���$��22(�6I   $
��Left=�6W   $��436,131,0,0q�6r   $	��Top=�6�   $��75��6�   $��37��6�   	 J� RenderTransformOrigin$	 0.316,0.415&�6�   $��1��6�   �� 5#   
   -   5        $��Rondalo)�5#      
��� ���$��22(�6:   $��Create��6H   $
��Left=�6Y   $��572,131,0,0q�6t   $	��Top=�6�   $��75��6�   $��37��6�   $	 0.316,0.415&�6�   $��1��6�   �� 5$   
   -   5        $ rName��5$      $��Enterr Room Name :��6   $��Rondalo)�6<   
�������$��
ExtraBlack"�6d   $
��Left=�6|   $��
94,136,0,0q�6�   $	��Top=�6�   $��1��6�   �� 5%   
   -   5        
 ��Name$

 room��5%      $
��Left=�6    $��23��6;   $��234,139,0,0q�6G    �� TextWrapping$
 Wrap=�6\   $�� ��6p   $	��Top=�6x   $	��120��6�   $	 0.365,0.057&�6�   $��1��6�   �� 5&   
   -   5        $��	listview1��5&      $��94,103,103,83q�6$   
�������$��2��6T    �� View 5'      �� 5(      .P�5)      �� $��RoomName��6%    �� Width$	 600��6d    �� DisplayMemberBinding 6:   �� *
RoomName+6d   5+      5-      50      51      