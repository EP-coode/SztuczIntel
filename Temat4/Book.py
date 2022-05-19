

class Book:
  def __init__(self, id, title, genres, description):
    self.id: str = id
    self.title: str = title
    self.description: str = description
    self.genres: list[str] = genres

  def __str__(self):
    return f"{self.id} - {self.title}, genres: {str(self.genres)}"
