import json
import numpy as np
from BookShelf import BookShelf
from bookValidators import books_containing_genres, description_longer_than, no_books_with_genres_count_more_than
from dataLoader import load_all_books_from_file
import os
import shutil
# from sklearn.feature_extraction.text import TfidfTransformer
# from sklearn.model_selection import train_test_split

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


book_shelf: BookShelf = load_all_books_from_file("./dataset/booksummaries.txt")

print(f"Input records count - {len(book_shelf.books)}")
print(f"Processing...")

book_shelf.remove_unvalid_books(description_longer_than(300))
book_shelf.remove_unvalid_books(no_books_with_genres_count_more_than(3))

book_shelf.merge_genres('Speculative fiction', 'Fiction')
book_shelf.merge_genres('Science Fiction', 'Fiction')
book_shelf.remove_genres('Fiction')

genres_cout = book_shelf.get_genres_count()
genres_cout = sorted(genres_cout.items(),
                     key=lambda item: item[1], reverse=True)

top_genres = list(map(lambda item: item[0], genres_cout[0:5]))
not_qualified_genres = list(map(lambda item: item[0], genres_cout[5:-1]))

book_shelf.remove_unvalid_books(books_containing_genres(top_genres))

for g in not_qualified_genres:
    book_shelf.remove_genres(g)

book_shelf.remove_genres('Fairytale fantasy')

books_by_category = book_shelf.get_books_grouped_by_category()

OUT_DIR = 'output'

if os.path.exists(OUT_DIR):
    shutil.rmtree(OUT_DIR)

os.mkdir(OUT_DIR)

for category in books_by_category.keys():
    os.mkdir(f"{OUT_DIR}/{category}")
    for book in books_by_category[category]:
        with open(f"{OUT_DIR}/{category}/{book.id}", "w", encoding="UTF-8") as file:
            file.write(prepare_document(book.description))
            file.close()
