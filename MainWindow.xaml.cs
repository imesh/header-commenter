/*
 *  Copyright 2013 Imesh Gunaratne
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 *  History: 
 *  2013/02/02 Created.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Imesh.Tools.HeaderCommentManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TxtSourceFolder.Text = Directory.GetCurrentDirectory();
            ChkApplyRecursively.IsChecked = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            try
            {
                string text = string.Empty;
                TxtLog.Text = string.Empty;
                SearchOption searchOption = SearchOption.TopDirectoryOnly;
                if (ChkApplyRecursively.IsChecked.Value)
                    searchOption = SearchOption.AllDirectories;
                var filePaths = Directory.GetFiles(TxtSourceFolder.Text, TxtSearchPattern.Text, searchOption);
                foreach (string filePath in filePaths)
                {
                    TxtLog.Text += String.Format("File: {0}", filePath);
                    if (ChkBackupOriginal.IsChecked.Value)
                    {
                        File.Copy(filePath, String.Format("{0}.bkp", filePath));
                        TxtLog.Text += String.Format("Backup file created");
                    }

                    text = File.ReadAllText(filePath);
                    if (ChkReplaceExisting.IsChecked.HasValue && ChkReplaceExisting.IsChecked.Value)
                    {
                        int start = text.IndexOf("/*");
                        int end = text.IndexOf("*/");                        
                        text = text.Substring(start, text.Length - end);
                    }
                    text = String.Format("{0}{1}{2}", TxtHeaderComment.Text, Environment.NewLine, text);

                    File.WriteAllText(filePath, text);
                    TxtLog.Text += String.Format("Header comment added");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
