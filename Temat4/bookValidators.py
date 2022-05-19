from typing import Callable
from Book import Book


def description_longer_than(desc_min_len: int) -> Callable[[Book], bool]:
  return lambda book: len(book.description) >= desc_min_len


def books_containing_genres(accepted_genres: list[str]) -> Callable[[Book], bool]:
  def book_contains_any_genre(book: Book) -> bool:
    for g in book.genres:
      if g in accepted_genres:
        return True

    return False

  return book_contains_any_genre


def no_books_with_genres_count_more_than(max_genres_count: int) -> Callable[[Book], bool]:
  return lambda book: len(book.genres) <= max_genres_count
