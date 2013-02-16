# IMPORTS
from utility import FileCache
import utility

import plp
from text_checker import default_encoding

# CLASS DEFINITION

class Entity(object):
    u"""Klasa przechowuje wszystkie informacje potrzebne do analizy slowa. Na podstawie struktur CLP"""
    def __init__(self):
        self.base = ""
        self.forms = []
        self.probability = 100.0
        self.label = ""
        self.prefix = ""
        self.original = ""

class WordStemmer(object):
    u"""Obiekt dostarczajacy mechanizmu stemmingu slow"""
    def __init__(self):
        self.__entities = []
        self.__fileCache = FileCache()
        self.__onlybase = {}

    def stemm(self, word):
        u"""Dokonuje stemmingu na slowie podanym w metodzie.
        Zwraca liste entity."""
        self.__entities = []
        if (word[len(word) - 1] == 'x'):
            utility.add_to_dict(self.__onlybase, word)
            return None

        similar_forms = self.__findSimilarForms(word)
        if similar_forms != None:
            self.__createEntities(word, similar_forms)
        else:
            utility.add_to_dict(self.__onlybase, word)
            return None

        return self.__entities

    def getOnlyBase(self):
        return self.__onlybase

    def __findSimilarForms(self, word):
        max = -1
        similar_forms = []

        for line in self.__fileCache.grabFile("formy.txt"):
            form = line.decode(default_encoding())
            sameCharsCount = self.__getCountCommonLettersFromEnd(word, form)
            if sameCharsCount > max and sameCharsCount > 1:
                max = sameCharsCount
                similar_forms = []
            if sameCharsCount == max:
                if form not in similar_forms:
                    similar_forms.append(form)
        if max > 1:
            return similar_forms
        else:
            return None

    def __getCountCommonLettersFromEnd(self, word, form):
        count = 0
        i = len(word) - 1
        j = len(form) - 1
        while (i >= 0 and j >= 0):
            if word[i] == form[j]:
                count += 1
                i -= 1
                j -= 1
            else:
                break
        return count

    def __createEntities(self, word, similar_forms):
        entities = []

        for similar_form in similar_forms:
            similar_form_entity = self.__loadEntityFromClp(similar_form)
            isDuplicated = False
            for entity in entities:
                if similar_form_entity.label == entity.label:
                    isDuplicated = True
                    break
            if isDuplicated:
                continue

            entities.append(similar_form_entity)
            self.__entities.append(self.__generateNewWordEntity(word, similar_form, similar_form_entity))

    def __generateNewWordEntity(self, word, similar_form, similar_form_entity):
        word_entity = Entity()
        postfix = similar_form[len(similar_form_entity.prefix):]
        if postfix == "":
            word_entity.prefix = word
        else:
            word_entity.prefix = word[:len(word) - len(postfix)]

        append_base_form = similar_form_entity.base[len(similar_form_entity.prefix):]
        word_entity.base = word_entity.prefix + append_base_form

        for form in similar_form_entity.forms:
            append_form = form[len(similar_form_entity.prefix):]
            word_entity.forms.append(word_entity.prefix + append_form)

        word_entity.label = similar_form_entity.label
        word_entity.probability = 100.0
        word_entity.original = similar_form_entity.base

        return word_entity

    def __loadEntityFromClp(self, similar_form):
        entity = Entity()
        for id in plp.plp_rec(similar_form.encode(default_encoding())):
            entity.base = plp.plp_bform(id).decode(default_encoding())
            entity.label = plp.plp_label(id).decode(default_encoding())
            for form in plp.plp_forms(id):
                entity.forms.append(form.decode(default_encoding()))
            break
        entity.prefix = self.__getPrefix(entity.base, entity.forms)

        return entity

    def __getPrefix(self, base, forms):
        prefix = base
        lookingForPrefix = True
        while (lookingForPrefix):
            lookingForPrefix = False
            for form in forms:
                if form.startswith(prefix):
                    continue
                lookingForPrefix = True
                break
            if lookingForPrefix:
                prefix = prefix[:len(prefix) - 1]
        return prefix
