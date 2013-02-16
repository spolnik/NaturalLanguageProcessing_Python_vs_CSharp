# IMPORTS

import plp
import re
import utility

# FIELDS

__results = {}
__shortcuts = {}
__rome = {}

# ENCODING FUNCTION

def default_encoding():
    u"""Kodowanie domyslne dla CLP - iso-8859-2"""

    return 'iso-8859-2';

def utf_encoding():
    u"""Kodowanie utf-8"""

    return 'utf-8';

# GETTERS

def getResults():
    return __results

def getShortcuts():
    return __shortcuts

def getRome():
    return __rome

# MAIN FUNCTION

def process(fileName, encoding):
    u"""Przetwarza plik zapisujac w __results slowa nie znalezione w clp.
W __shortcuts zapisywane sa prawdopodobne skroty.
Po kazdorazowym uruchomieniu __results i __shortcuts sa czyszczone.

fileName - nazwa pliku
encoding - kodowanie pliku
Return None
    """
    __results.clear()
    __shortcuts.clear()

    for line in open(fileName, "r"):
        # prepare line
        line = line.strip().decode(encoding)
        if re.match('^#\d{6}', line):
            continue

        # process line
        for word in re.split('\[|\]|\d+|\s+|[-&=#`;!.:?,\")(\'\\_/]', line):
            if len(word) <= 2:
                continue

            if not plp.plp_rec(word.encode(default_encoding())):
                __processWord(word)

# PRIVATE OPERATIONS

def __processWord(word):
    u"""Przetwarzanie slowa, sprawdzenie jego typu i dodanie go do odpowiedniej listy."""
    if len(word) == 3 or word.isupper():
        if __isRome(word):
            utility.add_to_dict(__rome, word)
        else:
            utility.add_to_dict(__shortcuts, word)
    else:
        utility.add_to_dict(__results, word)

def __isRome(word):
    u"""Sprawdza czy podane slowo jest liczba rzymska"""
    rome_letters = ['I', 'L', 'X', 'C', 'M', 'V', 'D']
    for letter in word:
        if letter not in rome_letters:
            return False
    return True

