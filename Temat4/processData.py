import json
import numpy as np
from BookShelf import BookShelf
from bookValidators import books_containing_genres, description_longer_than, no_books_with_genres_count_more_than
from dataLoader import load_all_books_from_file
import os
import shutil
import matplotlib.pyplot as plt

from dataPreprocesing import prepare_document

book_shelf: BookShelf = load_all_books_from_file("./dataset/booksummaries.txt")

books_count = len(book_shelf.books)
all_genres = list(book_shelf.get_genres_count().keys())

print(f"Input records count - {books_count}")
print(f"Processing...")

MAX_GENRES = 3
MIN_SUMMARY_LEN = 100
GENRES_TO_SELECT = ["Science Fiction",
                    "Children's literature", "Novel", "Crime Fiction", "Historical novel"]
# remove bad quality data

print(f"Removing books with genres count more than {MAX_GENRES}")
book_shelf.remove_unvalid_books(
  no_books_with_genres_count_more_than(MAX_GENRES))
print(f"Removed {books_count - len(book_shelf.books)} books")
books_count = len(book_shelf.books)

print(f"Removing books with summary shorter than {MIN_SUMMARY_LEN}")
book_shelf.remove_unvalid_books(description_longer_than(MIN_SUMMARY_LEN))
print(f"Removed {books_count - len(book_shelf.books)} books")
books_count = len(book_shelf.books)

# plot genres
print("Showing top 8 book genres count")
genres_cout = book_shelf.get_genres_count()
genres_cout = dict(sorted(genres_cout.items(),
                          key=lambda item: item[1], reverse=True)[0:8])
plt.bar(range(len(genres_cout)), list(genres_cout.values()), align='center')
plt.xticks(range(len(genres_cout)), list(genres_cout.keys()))
plt.show()

# remove unnecesary genres
not_qualified_genres = list(
  filter(lambda a: a not in GENRES_TO_SELECT, all_genres))

book_shelf.remove_unvalid_books(books_containing_genres(GENRES_TO_SELECT))

for g in not_qualified_genres:
  book_shelf.remove_genres(g)

print("Showing selected genres count")
genres_cout = book_shelf.get_genres_count()
genres_cout = dict(sorted(genres_cout.items(),
                          key=lambda item: item[1], reverse=True))
plt.bar(range(len(genres_cout)), list(genres_cout.values()), align='center')
plt.xticks(range(len(genres_cout)), list(genres_cout.keys()))
plt.show()


# save results
print(f"Output records: {len(book_shelf.books)}")
print("Saving...")

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
