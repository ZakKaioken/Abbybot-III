# Favorite Character Simplification Theory

First we parse input

if there's no input we show the character/list
if there is input, we check for if the first argument is one of the following mini commands
1. help
2. history
3. undo
4. revert
5. add
6. remove

help, history, undo and revert don't continue onto the normal fc functionality

Add means that we'll add to a users fc list
remove means we will remove a tag from the fc list

When we add or remove a title fc string will be created to show who is being added to the fc

We are maintaining two strings, one that marks what changed and one that holds the full value

Then we set the the tags arg based on what operation we're doing. if it's an add/remove we give the title fc, otherwise if it's a set we use the normal fc

Then we loop through the try catches to try to set the picture url. if it fails 3 times we give up and write an error message.


if it turns out that the picture contains 
we try to figure out what the operation was and use the titlefc or normal fc to be set into the historys info parameter.

We set the FC into abbybot's memory.
We also set the fc into the fc history.

finally, we make our embeds, using the operation we did and 


### noting what went wrong in this style

The problem with the whole concept of having mini argument commands, is that it's literally just the implementation of another command inside the current command. That has some problems that come with it, that may be solvable by just moving our code to another command. However that too comes with the problem of having to carve out functionality to move the different subcommands to their own files. 

That's why i'm going to introduce: 

## Subcommands/Subfunctionality

We do the testing/nontesting in this class, but move the functionality of each subcommand to the different files.

We have FC varients that use the same coding style, so having the split functinality will work better this way.