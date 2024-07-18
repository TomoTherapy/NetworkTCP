using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkTCP
{
    public partial class MainWindow : Window
    {
        private bool AutoScroll = true;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindow_ViewModel();

            Sample_textBlock.Text = "Love is patient, love is kind. It does not envy, it does not boast, it is not proud. It does not dishonor others, " +
                "it is not self-seeking, it is not easily angered, it keeps no record of wrongs. Love does not delight in evil but rejoices with the truth. " +
                "It always protects, always trusts, always hopes, always perseveres. Love never fails. But where there are prophecies, they will cease; " +
                "where there are tongues, they will be stilled; where there is knowledge, it will pass away.\nCorinthians 13:4 - 8";
        }

        private void ServerSend_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).ServerSend();
        }

        private void ServerClear_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).ServerClear();
        }

        private void ServerOpenClose_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).ServerOpenClose(Server_comboBox.Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).Closing();
        }

        private void ServerSendMsg_textBox_KeyUp(object sender, KeyEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).ServerSendMsgKeyUp(e);
        }

        private void Server_scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {
                if ((sender as ScrollViewer).VerticalOffset == (sender as ScrollViewer).ScrollableHeight)
                {
                    AutoScroll = true;
                }
                else
                {
                    AutoScroll = false;
                }
            }

            if (AutoScroll && e.ExtentHeightChange != 0)
            {
                (sender as ScrollViewer).ScrollToBottom();
            }
        }

        private void ServerPort_textBox_KeyUp(object sender, KeyEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).ServerPortKeyUp(Server_comboBox.Text, e);
        }

        private void ClientOpenClose_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).ClientOpenClose();
        }

        private void ClientSend_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).ClientSend();
        }

        private void ClientClear_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).ClientClear();
        }

        private void ClientMsg_textBox_KeyUp(object sender, KeyEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).ClientSendMsgKeyUp(e);
        }

        private void ClientPort_textBox_KeyUp(object sender, KeyEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).ClientIPPortKeyUp(e);
        }

        private void ClientIP_textBox_KeyUp(object sender, KeyEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).ClientIPPortKeyUp(e);
        }

        private void Client_scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {
                if ((sender as ScrollViewer).VerticalOffset == (sender as ScrollViewer).ScrollableHeight)
                {
                    AutoScroll = true;
                }
                else
                {
                    AutoScroll = false;
                }
            }

            if (AutoScroll && e.ExtentHeightChange != 0)
            {
                (sender as ScrollViewer).ScrollToBottom();
            }
        }

        private void SetDefault_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindow_ViewModel).SetDefault_button_Click();
        }
    }
}
