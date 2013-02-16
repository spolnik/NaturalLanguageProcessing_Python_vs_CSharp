# IMPORTS
import string

# UTILITY OPERATIONS

def add_to_dict(dict, word):
    u"""Add word to dictionary.
     * Count 1 if it is first time.
     * Count ++ it it is next time.
    Return None.
    """
    if word in dict:
        dict[word] += 1
    else:
        dict[word] = 1

class FileCache:
    '''Caches the contents of a set of files.
    Avoids reading files repeatedly from disk by holding onto the
    contents of each file as a list of strings.
    '''

    def __init__(self):
        self.filecache = {}

    def grabFile(self, filename):
        '''Return the contents of a file as a list of strings.
        New line characters are removed.
        '''
        if not self.filecache.has_key(filename):
            f = open(filename, "r")
            self.filecache[filename] = string.split(f.read(), '\n')
            f.close()
        return self.filecache[filename]
