# coding: utf-8
from stemming import WordStemmer
import sys
import text_checker
import plp

plp.plp_init()
# change encoding - encoding input
text_checker.process(sys.argv[1], text_checker.utf_encoding())

file = open(sys.argv[2], 'w')

file.write("===SHORTCUTS===\n")
for key in text_checker.getShortcuts().keys():
    file.write(u'SHORTCUT: %s, COUNT %d\n' % (key, text_checker.getShortcuts()[key]))

file.write("===ROME NUMBERS===\n")
for key in text_checker.getRome().keys():
    file.write(u'ROME: %s, COUNT %d\n' % (key, text_checker.getRome()[key]))

file.write("==TO DEBUG - RESULTS===\n")
for key in text_checker.getResults().keys():
    file.write(u'[debug] result: %s, COUNT %d\n' % (key, text_checker.getResults()[key]))

file.write("===NEW WORDS===\n")
word_stemmer = WordStreammer()

generated_forms = []
for key in text_checker.getResults().keys():

    if key in generated_forms:
        file.write(u'GENEZA: %s\n' % (key))
        file.write(u'EXISTED IN NEW WORDS\n')
        file.write("==================================\n")
        continue

    word_entities = word_stemmer.stemm(key)
    if word_entities == None:
        continue

    file.write(u'GENEZA: %s\n' % (key))

    for entity in word_entities:
        file.write(u'BASE: %s\n' % (entity.base))
        file.write(u'LABEL: %s\n' % (entity.label))

        file.write('FORMS: ')
        for form in entity.forms:
            file.write(form + ", ")
            generated_forms.append(form)
        file.write('\n')
        file.write(u'ODMIANA WG: %s\n' % (entity.original))
        file.write("==================================\n")

for key in word_stemmer.getOnlyBase().keys():
    file.write(u'GENEZA: %s\n' % (key))
    file.write(u'ONLY BASE: %s\n' % (key))
    file.write("==================================\n")
file.close()
