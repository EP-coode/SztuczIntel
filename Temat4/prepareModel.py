import numpy as np
from sklearn.datasets import load_files
from sklearn.feature_extraction.text import TfidfVectorizer, CountVectorizer
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import classification_report, confusion_matrix, accuracy_score
import nltk
from nltk.corpus import stopwords
import re
from sklearn.naive_bayes import GaussianNB

# from processData import prepare_document

movie_data = load_files("./output")
X, y = movie_data.data, movie_data.target

tfidfconverter = TfidfVectorizer(
  max_features=1000, min_df=0.01, max_df=0.4, stop_words=stopwords.words('english'))

X = tfidfconverter.fit_transform(X).toarray()

X_train, X_test, y_train, y_test = train_test_split(
  X, y, test_size=0.2, random_state=40, shuffle=True)

classifier = RandomForestClassifier()
classifier.fit(X_train, y_train)

y_pred = classifier.predict(X_test)

print(confusion_matrix(y_test, y_pred))
print(classification_report(y_test, y_pred))
print(accuracy_score(y_test, y_pred))
