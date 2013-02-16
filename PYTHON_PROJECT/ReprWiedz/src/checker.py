#!/usr/bin/python
#-*- coding: utf-8 -*-
from plp import *
import sys
import re
import operator
import utility

list = {}
plp_init()
should_be_utf = False

if (sys.argv[2] == 'u'):
    should_be_utf = True

for line in open(sys.argv[1], "r"):
    for word in re.split("\[|\]|\\d+|\\s+|[-=`;!.:?,\")(\'\\_/]", line.decode('iso-8859-2').lower()):
        if len(word) == 0:
            continue

        word = word.encode('iso-8859-2')
        if not plp_rec(word):
            if should_be_utf:
                utility.add_to_dict(list, word.decode('iso-8859-2'))
            else:
                utility.add_to_dict(list, word)

for key, value in sorted(list.iteritems(), key = operator.itemgetter(1), reverse = True):
    print(key + ' : ' + str(value))
