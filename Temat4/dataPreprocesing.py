import re
from nltk.stem import WordNetLemmatizer
# nltk.download('stopwords')
# nltk.download('wordnet')
# nltk.download('omw-1.4')

stemmer = WordNetLemmatizer()

def prepare_document(doc):
   # Remove all the special characters
  document = re.sub(r'\W', ' ', str(doc))

  # remove all single characters
  document = re.sub(r'\s+[a-zA-Z]\s+', ' ', document)

  # Remove single characters from the start
  document = re.sub(r'\^[a-zA-Z]\s+', ' ', document)

  # Substituting multiple spaces with single space
  document = re.sub(r'\s+', ' ', document, flags=re.I)

  # Removing prefixed 'b'
  document = re.sub(r'^b\s+', '', document)

  # Converting to Lowercase
  document = document.lower()

  # Lemmatization
  document = document.split()

  document = [stemmer.lemmatize(word) for word in document]
  document = ' '.join(document)

  return document
