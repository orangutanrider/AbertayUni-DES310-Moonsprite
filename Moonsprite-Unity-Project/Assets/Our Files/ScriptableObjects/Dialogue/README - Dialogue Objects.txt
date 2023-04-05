(as of writing this, 230405)
Dialogue objects hold the following fields:
	dialogueSpeaker
	dialogueText
	nextDialogueObjects

	triggerDialogueEventsOnDialogueGiver
	triggerDialogueEventsOnPlayer
	customTags

========

dialogueSpeaker is a field for dialogueSpeakerScriptableObjects
These objects hold data pertaining to the person giving the dialogue, stuff like the sprite file for their 
avatar and their name.

Dialogue text is where you type in the dialogue itself.

nextDialogueObjects is a list for containing dialogueObjects
This list is where you put in the dialogue objects that come next. The reason it's a list is for supporting 
branching dialogue.
So, if the dialogue doesn't branch then you put in one dialogue object and this object will come after.
If the dialogue ends after the object in question, then leave the list empty.
And if it branches here then add multiple entries, one for each option.

The way replies are selected is with the number keys on your keyboard.
The 'Alphanumeric keys'.

The two trigger fields are bools (true or false), they are check boxes.
Checking these tells the dialogue system to attempt to trigger dialogue events on either the player or the game object
that told it to start the dialogue object.
The dialogue events themselves are any script that have implemented the IDialogueEvent interface.

The custom tags is a list of tags, and tags are just strings.
When a dialogue event is triggered, it will pass the tag list of the dialogue object to the scripts that have implemented 
the IDialogueEvent interface.
The reason it does this is so we can do more with the events. It allows us to have the event script only trigger if it recieves
a specific tag.