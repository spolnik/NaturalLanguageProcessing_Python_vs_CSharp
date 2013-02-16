#!/usr/bin/python

from plp import *
import sys

bases = []
plp_init()

should_be_utf = False

if (sys.argv[1] == 'u'):
    should_be_utf = True

for line in open("formy.txt", "r"):
    line = line.strip()
    for id in plp_rec(line):
        if id in bases:
            continue
        bases.append(id)

if should_be_utf:
    for id in bases:
        print "%d %s %s" % (id, str(plp_bform(id)).decode('iso-8859-2'), str(plp_label(id)), str(plp_vec(id)))
        print str(plp_forms(id)).decode('iso-8859-2')
else:
    for id in bases:
        print "%d %s %s" % (id, str(plp_bform(id)), str(plp_label(id)), str(plp_vec(id)))
        print str(plp_forms(id))
