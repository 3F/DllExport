/*!
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using net.r_eg.DllExport.Wizard.UI.Extensions;

namespace net.r_eg.DllExport.Wizard.UI.Controls
{
    public partial class ProgressLineControl: UserControl
    {
        private volatile bool stop;

        public void StartTrainEffect(int lenLimit, int step = 40, int delay = 50)
        {
            if(!stop) {
                return;
            }
            stop = false;

            int direction = 0; // for an complete processing per step
            void animate()
            {
                if(Width >= lenLimit) 
                {
                    direction = -1;
                    Left += step;

                    if(Left >= lenLimit) {
                        ResetTrainEffect();
                    }
                }
                else 
                {
                    direction = 1;
                    Width += step;
                }
            };

            Task.Factory.StartNew(() => 
            {
                while(true)
                {
                    this.UIAction(animate);

                    if(stop && direction < 0) {
                        this.UIAction(ResetTrainEffect);
                        return;
                    }

                    Thread.Sleep(delay);
                }
            });
        }

        public void StopAll()
        {
            stop = true;
        }

        public ProgressLineControl()
        {
            InitializeComponent();
            StopAll();
        }

        protected void ResetTrainEffect()
        {
            Width = 0;
            Left = 0;
        }

        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            StopAll();

            if(disposing && (components != null)) {
                components.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
