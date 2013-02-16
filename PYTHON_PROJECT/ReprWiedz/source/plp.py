# Python CLP Wrapper
# (c) Krzysztof Dorosz 
# 2008 AGH, dorosz@agh.edu.pl
from ctypes import *

CLPLIB = CDLL("libclp.so")        # Ladowanie biblioteki CLP
CLPLIB.clp_ver.restype = c_char_p    # Ustawienie typu zwracanego przez funkcje clp_ver
Array50 = c_int * 50            # Typ pomocniczy dla buforow (tablic) int 

def plp_init():
    """Inicjalizuje bibiloteke CLP"""
    CLPLIB.clp_init(0)

def plp_ver():
    """Zwraca napis z numerem wersji CLP"""
    return CLPLIB.clp_ver()

def plp_rec(word):
    """Zwraca liste numerow ID dla danego slowa"""
    ids = Array50()
    num = c_int(0)
    CLPLIB.clp_rec(word, ids, byref(num))
    return ids[0:num.value]

def plp_label(id):
    """Zwraca etykiete dla danego ID"""
    label = create_string_buffer(10)
    CLPLIB.clp_label(c_int(id), label)
    return label.value

def plp_bform(id):
    """Zwraca etykiete dla danego ID"""
    bform = create_string_buffer(80)
    CLPLIB.clp_bform(c_int(id), bform)
    return bform.value

def plp_forms(id):
    """Zwraca liste form dla danego wyrazu"""
    formy = create_string_buffer(2048)
    CLPLIB.clp_forms(c_int(id), formy)
    return formy.value.split(':')[0:-1]

def plp_vec(id, word):
    """Zwraca wector odmiany"""
    out = Array50()
    num = c_int(0)
    CLPLIB.clp_vec(c_int(id), word, out, byref(num))
    return out[0:num.value]
