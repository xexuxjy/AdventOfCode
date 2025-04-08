def welcome():
    print("Welcome!")
    

def countWords(theSentence):
    tokens = theSentence.split()
    return len(tokens)


userInput = input("Enter a sentence : ")

numWords = countWords(userInput)

print("Thera are "+str(numWords)+" in the sentence : "+userInput)


