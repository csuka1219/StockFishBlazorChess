using System.Diagnostics;
using System;

namespace StockFishBlazorChess.Services
{
    public class StockfishService
    {
        private StreamReader strmReader = default!;
        private StreamWriter strmWriter = default!;
        private Process stockfishProcess;

        public StockfishService()
        {
            //TODO: need add method which should be depended on os version
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "stockfish-windows-x86-64-avx2.exe",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };
            stockfishProcess = new Process { StartInfo = processStartInfo };
        }


        public void sendCommand(string command)
        {
            strmWriter?.WriteLine(command);
            strmWriter?.Flush();
        }

        public void wait(int millisecond)
        {
            stockfishProcess.WaitForExit(millisecond);
        }

        public void stopEngine()
        {
            sendCommand("quit");
            stockfishProcess.Kill();
            strmReader.Close();
            strmWriter.Close();
        }

        public void startEngine()
        {
            stockfishProcess.Start();

            strmWriter = stockfishProcess.StandardInput;
            strmReader = stockfishProcess.StandardOutput;

            strmWriter.WriteLine("ucinewgame");
        }

        public string readLine()
        {
            return strmReader.ReadLine()!;
        }
    }
}
