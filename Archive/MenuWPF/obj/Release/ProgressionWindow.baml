   M S B A M L   `   `   `  ����   MenuWPF    MenuWPF.ProgressionWindow   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    >d2http://schemas.microsoft.com/expression/blend/2008    Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    -     �Name$  Window�� � Title$ Loading��$	��800��$	��150�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=� � Icon$ ORTS.ico��#��� �����	#00000000 � WindowStyle$
 None=� � AllowsTransparency . .���� -    ��Name$ brdMain��
���   �$��1q����	#7FFFFFFF �� CornerRadius$ 20��$��	0,0,20,20q�	 J� Effect	 3  -System.Windows.Media.Effects.DropShadowEffect  
   
BlurRadius$
 10Q�   ShadowDepth$ 10Q�._�� $��0,0,-5,0q� � RowDefinitions �� $��40 ���  � ColumnDefinitions �� $	��120 �.+��� -    ��Name$ lblPath��$ ��Loading train database ...��$��Right=�$��
0,0,-200,0q�{�.  #��� ��$
��Bold"�
���   �$��16(�$��Bottom=�$	��300��� -    �Name$ progBar��$��30��$��20,8,-637,0q�$��1��$	��Top=�����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FF0090FF�� � Offset$ 0Q�� $ 	#FF65FF00��$ 1Q�