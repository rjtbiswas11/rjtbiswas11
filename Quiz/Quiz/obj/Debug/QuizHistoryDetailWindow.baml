   M S B A M L   `   `   `  ����  QuizApplication-    'QuizApplication.QuizHistoryDetailWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  QuizHistoryDetailWindow��5   	   $	��600��6)   $	��600��66    � WindowStartupLocation$ CenterScreen=�6B   .��5      � .+�5   
   �� -   5         ��Name$ tabQuestion��5      .@�5      �� -   5         ��Name$ tabitem_correct��5       �� 
IsSelected . $��Correct��6A   
�������.��5      �  � RowDefinitions 5	      �� 5
      $��* �6(   �� 5      $
��auto �6(   .+�5      �� -   5         ��Name$ 
sc_Correct��5   #   $
��Auto=�67   $��0��6Z   .��5      �� -   5         ��Name$ spl_Correct��5   %    �� Orientation$ Vertical=�6:   $��0 20 0 0q�6Q   5      5      ]� 5      $��1��6    .+�5      �� $ 
Horizontal=�6%   $��Right=�6>   .+�5      �� -   5        	 ��Name$	 btnCorrect_Prev��5   %   $
��Prev��6>   $��5 5 20 5q�6M   $��50��6_   $��20��6j   $��Right=�6v   $y�	Collapsed=�6�   �� 5      -   5        $	 btnCorrect_Next��5   %   $
��Next��6>   $��5 5 20 5q�6M   $��50��6_   $��20��6j   $��Right=�6v   $y�	Collapsed=�6�   5      5      5      5      �� 5      -   5        $ tabitem_wrong��5      $��Wrong��6-   
�������.��5      �  5      �� 5      $��* �6(   �� 5      $
��auto �6(   .+�5      �� -   5        $ sc_Wrong��5   #   $
��Auto=�65   $��0��6X   .��5       �� -	   5        $ 	spl_Wrong��5    %   $ Vertical=�68   $��0 20 0 0q�6O   5!      5"      ]� 5#      $��1��6    .+�5$      �� $ 
Horizontal=�6%   $��Right=�6>   .+�5%      �� -
   5        $	 btnWrong_Prev��5%   %   $
��Prev��6<   $��5 5 20 5q�6K   $��50��6]   $��20��6h   $��Right=�6t   $y�	Collapsed=�6�   �� 5&      -   5        $	 btnWrong_Next��5&   %   $
��Next��6<   $��5 5 20 5q�6K   $��50��6]   $��20��6h   $��Right=�6t   $y�	Collapsed=�6�   5'      5(      5)      5*      5+      5,      5-      