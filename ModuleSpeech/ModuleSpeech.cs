using System;
using System.Collections.Generic;
using System.Speech.Recognition;
using System.Speech.Synthesis;

using Computer.API;

namespace ModuleSpeech
{
    public class ModuleSpeech : ModuleBase
    {
        [Setting]
        private string voice = "IVONA 2 Ivy";
        [Setting]
        private double timeoutBabble = 1.0;
        [Setting]
        private double timeoutSilenceEnd = 1.0;
        [Setting]
        private double timeoutSilenceInitial = 1.0;
        [Setting]
        private double timeoutSilenceAmbiguous = 1.0;

        public static ModuleSpeech Instance { get; private set; }
        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer synthesizer;
        private Dictionary<string, Action<string[]>> library;

        public ModuleSpeech()
        {
            ModuleSpeech.Instance = this;
            this.recognizer = new SpeechRecognitionEngine();
            this.synthesizer = new SpeechSynthesizer();
            this.library = new Dictionary<string, Action<string[]>>();
        }
        protected override void Load()
        {
            this.recognizer.SetInputToDefaultAudioDevice();
            this.recognizer.BabbleTimeout = TimeSpan.FromSeconds(this.timeoutBabble);
            this.recognizer.EndSilenceTimeout = TimeSpan.FromSeconds(this.timeoutSilenceEnd);
            this.recognizer.InitialSilenceTimeout = TimeSpan.FromSeconds(this.timeoutSilenceInitial);
            this.recognizer.EndSilenceTimeoutAmbiguous = TimeSpan.FromSeconds(this.timeoutSilenceAmbiguous);

            this.synthesizer.SetOutputToDefaultAudioDevice();
            this.synthesizer.SelectVoice(this.voice);

            //this.recognizer.SpeechDetected += recognizer_SpeechDetected;
            //this.recognizer.SpeechHypothesized += recognizer_SpeechHypothesized;
            //this.recognizer.SpeechRecognitionRejected += recognizer_SpeechRecognitionRejected;
            this.recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            this.recognizer.RecognizeCompleted += Recognizer_RecognizeCompleted;
            this.recognizer.AudioStateChanged += Recognizer_AudioStateChanged;
            this.recognizer.AudioSignalProblemOccurred += Recognizer_AudioSignalProblemOccurred;
            this.recognizer.AudioLevelUpdated += Recognizer_AudioLevelUpdated;

            this.recognizer.SpeechRecognized += Recognizer_Info;
            this.recognizer.SpeechRecognitionRejected += Recognizer_Info;
            this.recognizer.SpeechHypothesized += Recognizer_Info;
        }
        protected override void Start()
        {
            if (this.recognizer.Grammars.Count == 0)
            {
                throw new Exception("No grammar loaded");
            }
            this.recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        public static void Speak(string sText)
        {
            ModuleSpeech.Instance.Log("Speaking: " + sText);
            ModuleSpeech.Instance.synthesizer.Speak(sText);
        }
        internal static void AddGrammar(Grammar grammar, Action<string[]> action)
        {
            ModuleSpeech.Instance.Log("Loading grammar: " + grammar.Name);
            ModuleSpeech.Instance.recognizer.LoadGrammar(grammar);
            //ModuleSpeech.Instance.recognizer.LoadGrammarAsync(grammar);
            ModuleSpeech.Instance.library.Add(grammar.Name, action);
        }
        #region "Events"
        private void Recognizer_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e) //EventArgs
        {

        }

        private void Recognizer_AudioSignalProblemOccurred(object sender, AudioSignalProblemOccurredEventArgs e) //EventArgs
        {

        }

        private void Recognizer_AudioStateChanged(object sender, AudioStateChangedEventArgs e) //EventArgs
        {

        }

        private void Recognizer_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {

        }
         
        private void Recognizer_Info(object sender, RecognitionEventArgs e)
        {
            string tag = e.GetType().Name.Replace("Speech", "").Replace("EventArgs", "");
            float conf = e.Result.Confidence;
            string name = "";
            if (e.Result.Grammar != null)
            {
                name = $" {e.Result.Grammar.Name}:";
            }
            Log($"Speech {tag} {conf:00.0%}{name} {e.Result.Text}");
        }
        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e) //RecognitionEventArgs
        {
            //Action<string[]> action;
            //if (!library.TryGetValue(e.Result.Grammar.Name, out action))
            //{
            //    LogErr("Action not found: " + e.Result.Grammar.Name);
            //    return;
            //}
            //Log("Running action for " + e.Result.Grammar.Name);
            //action(null);
        }

        ////private void recognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e) //RecognitionEventArgs
        ////{

        ////}

        ////private void recognizer_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e) //RecognitionEventArgs
        ////{

        ////}
        ////private void recognizer_SpeechDetected(object sender, SpeechDetectedEventArgs e) //EventArgs
        ////{

        ////}
        #endregion
    }
}
