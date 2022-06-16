from operator import mod
from select import select
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
from sklearn.feature_selection import SelectKBest, VarianceThreshold, chi2, f_classif, mutual_info_classif
import csv

from sklearn.svm import SVC

# Konsultacje: 13.06; 20.06

# Naive Bayes
# P(klasaX, wektorCech) = (P(klasaX)*P(wektorCech | klasaX)) / P(wektorCech)
# Upraszczając: klasa = max(P(klasaX)*iloczyn(P(cechaX | klasaX)))
# gatunek = max(P(gatunkuX) * iloczyn(P(cechaX | gatunkuX)))

# parametr alfa wygładza model eliminując ekstremalne przypadki,
# np. wszyskie cechy wskasują wysoce na przynależność do danej klazy
# jednak jedna z nich wyklucza przynależność ponieważ p(klasaX | cechaX) = 0

# SVM - support vector machines
#  staramy się wyznaczyć hiperpłaszczyznę o N-1 wymiarach
#  która rozszieli nam zbiory należące do różnych kategorii.
#  kernel - dodaje do danych wymiar który
#  powstaje z przkształcenia danych tak,
#  aby klasyfikator mógł podzielić je hiperpłaszczyzną


def test_model(X, y, model=MultinomialNB(alpha=1), n_splits=4, test_size=0.3, max_features=5000, fatures_after_selection=1000):
  count_vectorizer = CountVectorizer(
    max_features=max_features, max_df=0.5, ngram_range=(1, 2), stop_words=stopwords.words('english'))

  # selector1 = VarianceThreshold(threshold=(
  #  max_feature_prob * (1 - max_feature_prob)))

  # chi2 pozwala na wyznaczenie zależności gatunku od danych cech
  # selektor wybiera k cech mających największy wpływ na gatunek
  selector2 = SelectKBest(chi2, k=fatures_after_selection)

  print("Vectorizing data...")
  X = count_vectorizer.fit_transform(X).toarray()
  X = selector2.fit_transform(X, y)

  print("Splitting data...")
  # zachowuje proporcje w podzbiorach
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

  return [str(model), str(n_splits), str(test_size), str(max_features), str(worst_score), str(best_score), str(fatures_after_selection)]


print("Loading data...")
movie_data = load_files("./output")
X, y = movie_data.data, movie_data.target


# ========================
# outcome_rows = [["model", "n_splits", "test_size", "max_features",
#                  "worst_score", "best_score", "fatures_after_selection"]]

# print("Performing experiments...")
# fatures_after_selection_set = [50, 100, 500, 1000, 1500, 2000]
# for fatures_after_selection in fatures_after_selection_set:
#   outcome = test_model(X, y, max_features=10000,
#                        fatures_after_selection=fatures_after_selection)
#   outcome_rows.append(outcome)

# print("Saving results...")
# with open("./results/features_count.csv", 'w+') as file:
#   writer = csv.writer(file)
#   writer.writerows(outcome_rows)


# ========================
# outcome_rows = [["model", "n_splits", "test_size", "max_features",
#                  "worst_score", "best_score", "fatures_after_selection"]]

# print("Performing experiments...")
# alphas = [0, 1, 10, 20, 50, 100]
# for alpha in alphas:
#   outcome = test_model(X, y, max_features=10000,
#                        fatures_after_selection=1500, model=MultinomialNB(alpha=alpha))
#   outcome_rows.append(outcome)

# print("Saving results...")
# with open("./results/alpha.csv", 'w+') as file:
#   writer = csv.writer(file)
#   writer.writerows(outcome_rows)

# ========================

# outcome_rows = [["model", "n_splits", "test_size", "max_features",
#                  "worst_score", "best_score", "fatures_after_selection"]]

# print("Performing experiments...")
# kernels = ['linear', 'poly', 'sigmoid']
# for kernel in kernels:
#   outcome = test_model(X, y, max_features=5000,
#                        fatures_after_selection=1000, model=SVC(kernel=kernel))
#   outcome_rows.append(outcome)

# print("Saving results...")
# with open("./results/kernels.csv", 'w+') as file:
#   writer = csv.writer(file)
#   writer.writerows(outcome_rows)


# ========================

# outcome_rows = [["model", "n_splits", "test_size", "max_features", "worst_score", "best_score", "fatures_after_selection"]]

# training_models = [SVC(kernel="rbf"), SVC(kernel="sigmoid"), RandomForestClassifier(
# ), GaussianNB(), MultinomialNB(), BernoulliNB(),  ComplementNB()]
# print("Performing experiments...")
# for model in training_models:
#   outcome = test_model(X, y, model=model)
#   outcome_rows.append(outcome)

# print("Saving results...")
# with open("./results/training_model.csv", 'w+') as file:
#   writer = csv.writer(file)
#   writer.writerows(outcome_rows)

test_model(X, y, model=MultinomialNB(alpha=10),
           max_features=10_000, fatures_after_selection=2000)

test_model(X, y, model=SVC(kernel='linear'),
           max_features=10_000, fatures_after_selection=2000)