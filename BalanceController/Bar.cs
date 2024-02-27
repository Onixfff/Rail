using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rail.BalanceController
{
    internal class Bar
    {
        public Action onProgress;
        public Action onError;

        private ProgressBar _progressBar;

        private int _lastProgress = 0;
        private int _progress;
        private const int _endProgress = 100;

        public Bar(ProgressBar progressBar)
        {
            _progressBar = progressBar;
        }

        public void ShowBar()
        {
            onProgress += ChangeValueBar;
            onError += Error;
            _progressBar.Invoke((MethodInvoker)delegate
            {
                _progressBar.Visible = true;
            });
            Task.Run(() => ChangeBar());
        }

        private void ChangeBar()
        {
            _progress = 20;


            while (_progressBar.Visible)
            {
                int value = 0;
                for (int i = _lastProgress; i <= _progress; i++)
                {
                    value = i;
                    _progressBar.Invoke((MethodInvoker) delegate
                    {
                        _progressBar.Value = i;
                    });
                    
                    if(i == _progress)
                    {
                        _lastProgress = _progress;
                    }
                }

                if (value == _endProgress)
                {
                    _progressBar.Invoke((MethodInvoker)delegate
                    {
                        _progressBar.Visible = false;
                    });

                    onProgress -= ChangeValueBar;
                    onError -= Error;
                    balance.onCompliteMove.Invoke();
                }
            }
        }

        private void Error()
        {
            _progress = _endProgress;
            //balance.onUpdate.Invoke();
            //TODO Сделать при Error возврат кнопки перемещения (button1)
            //balance.onCompliteMove.Invoke();
        }

        private void ChangeValueBar()
        {
            switch (_progress)
            {
                case 20:
                    _progress = 50;
                    balance.onUpdate.Invoke();
                    break;
                case 50:
                    _progress = _endProgress;
                    break;
                default:
                    _progress = _endProgress;
                    break;

            }
        }


    }
}
