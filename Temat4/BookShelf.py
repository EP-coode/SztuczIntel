
from typing import Callable
from Book import Book


class BookShelf:
  def __init__(self) -> None:
    self.books: list[Book] = []

  def add_book(self, book: Book):
    self.books.append(book)

  def get_genres_count(self) -> dict[str, int]:
    genres_dict: dict[str, int] = {}

    for book in self.books:
      for genre in book.genres:
        if genres_dict.get(genre) is None:
          genres_dict[genre] = 0
        else:
          genres_dict[genre] += 1

    return genres_dict

  def remove_genres(self, genre: str):
    for book in self.books:
      if genre in book.genres:
        book.genres.remove(genre)

  def remove_unvalid_books(self, book_validator: Callable[[Book], bool]):
    books_to_remove: list[Book] = []

    for book in self.books:
      if not book_validator(book):
        books_to_remove.append(book)

    self.books = list(
      filter(lambda book: book not in books_to_remove, self.books))
