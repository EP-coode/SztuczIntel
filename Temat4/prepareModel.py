import numpy as np
from regex import splititer
from sklearn.datasets import load_files
from sklearn.feature_extraction.text import TfidfVectorizer, CountVectorizer
from sklearn.model_selection import StratifiedShuffleSplit, train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import classification_report, confusion_matrix, accuracy_score
import nltk
from nltk.corpus import stopwords
import re
from sklearn.naive_bayes import BernoulliNB, ComplementNB, GaussianNB, MultinomialNB
import csv

from sklearn.svm import SVC

# from processData import prepare_document


def test_model(X, y, model=RandomForestClassifier(), n_splits=5, test_size=0.3, min_df=0.01, max_df=0.4, max_features=1000):
  tfidfconverter = CountVectorizer(
    max_features=max_features, min_df=min_df, max_df=max_df, ngram_range=(1, 2), stop_words=stopwords.words('english'))

  print("Vectorizing data...")
  X = tfidfconverter.fit_transform(X).toarray()

  print("Splitting data...")
  sss = StratifiedShuffleSplit(
    n_splits=n_splits, test_size=test_size, random_state=0)
  sss.get_n_splits(X, y)

  best_score = None
  worst_score = None

  for train_index, test_index in sss.split(X, y):
    X_train, X_test = X[train_index], X[test_index]
    y_train, y_test = y[train_index], y[test_index]
    model.fit(X_train, y_train)
    prediction = model.predict(X_test)
    score = accuracy_score(y_test, prediction)
    print(f"Score: {score}")

    if best_score == None or best_score < score:
      best_score = score

    if worst_score == None or worst_score > score:
      worst_score = score

  return [str(model), str(n_splits), str(test_size), str(min_df), str(max_df), str(max_features), str(worst_score), str(best_score)]


print("Loading data...")
movie_data = load_files("./output")
X, y = movie_data.data, movie_data.target
outcome_rows = [["model", "n_splits", "test_size", "min_df",
                 "max_df", "max_features", "worst_score", "best_score"]]

print("Performing experiments...")
fetures_count = [100, 500, 1000, 1500, 2000, 2500]
for max_features in fetures_count:
  outcome = test_model(X, y, max_features=max_features)
  outcome_rows.append(outcome)

print("Saving results...")
with open("./results/features_count.csv", 'w+') as file:
  writer = csv.writer(file)
  writer.writerows(outcome_rows)

outcome_rows = [["model", "n_splits", "test_size", "min_df",
                 "max_df", "max_features", "worst_score", "best_score"]]

training_models = [SVC(kernel="rbf"), SVC(kernel="sigmoid"), RandomForestClassifier(
), GaussianNB(), MultinomialNB(), BernoulliNB(),  ComplementNB()]
print("Performing experiments...")
for model in training_models:
  outcome = test_model(X, y, model=model)
  outcome_rows.append(outcome)

print("Saving results...")
with open("./results/training_model.csv", 'w+') as file:
  writer = csv.writer(file)
  writer.writerows(outcome_rows)

# y_pred = classifier.predict(X_test)

# print(confusion_matrix(y_test, y_pred))
# print(classification_report(y_test, y_pred))
# print(accuracy_score(y_test, y_pred))

# na następny tydzień

# 3 etap
# 3 eksperymenty z listy, 3 własne
