https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/
(at "How to fade an Audio Mixer Group?")

========
Create a audio mixer for your sounds 
(right click in project explorer, then click create)
You can re-use audio mixers, just create one per group of sounds (Ambience, Music, ect.)

Double click the audio mixer
Create a new group for the audio you wanna fade
Click on the group in the audio mixer tab
Specifically right click the "volume" text, under attenuation (for the group in the inspector)
Click expose in the right click menu
There should now be an arrow next to the volume parameter

If you go back to the audiomixer window you should now see that in the top right Of it the ExposedParameters drop down has incremented
Click that drop down and find the paramter you exposed, then rename it to something appropriate

You can now fade/lerp that parameter by using the FadeMixerGroup class
Example:
StartCoroutine(FadeMixerGroup.FadeMixerGroup01Volume(targetAudioMixer, AudMixerParamReferences.testFadeVolume, 0, fadeDuration));

We have to reference the parameter via a string
AudMixerParamReferences.testFadeVolume
This line is a const string, it is stored in the 
AudMixerParamRefrences class, use this class to store all your string references.