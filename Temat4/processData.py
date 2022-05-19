import json
from BookShelf import BookShelf
from bookValidators import books_containing_genres, description_longer_than, no_books_with_genres_count_more_than
from dataLoader import load_all_books_from_file


book_shelf: BookShelf = load_all_books_from_file("./dataset/booksummaries.txt")

print(f"Input records count - {len(book_shelf.books)}")
print(f"Processing...")

book_shelf.remove_unvalid_books(description_longer_than(100))
book_shelf.remove_unvalid_books(no_books_with_genres_count_more_than(5))
genres_cout = book_shelf.get_genres_count()
genres_cout = sorted(genres_cout.items(),
                     key=lambda item: item[1], reverse=True)
top_genres = list(map(lambda item: item[0], genres_cout[0:5]))
not_qualified_genres = list(map(lambda item: item[0], genres_cout[5:-1]))
book_shelf.remove_unvalid_books(books_containing_genres(top_genres))

for g in not_qualified_genres:
  book_shelf.remove_genres(g)

print(f"Output records count - {len(book_shelf.books)}")
print("Saving...")

with open("output.json", "w") as f:
  books_dict = [book.__dict__ for book in book_shelf.books]
  json_data = json.dumps(books_dict, indent=2)
  f.write(json_data)
  f.close()
