import numpy as np
from sklearn.datasets import load_files
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import classification_report, confusion_matrix, accuracy_score
import nltk
from nltk.corpus import stopwords
import re
from sklearn.naive_bayes import GaussianNB

from processData import prepare_document


movie_data = load_files("./output")
X, y = movie_data.data, movie_data.target

tfidfconverter = TfidfVectorizer(
    max_features=500, min_df=0.5, max_df=0.7, stop_words=stopwords.words('english'))

X = tfidfconverter.fit_transform(X).toarray()

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.33, random_state=40, shuffle=True)

classifier = GaussianNB()
classifier.fit(X_train, y_train)

y_pred = classifier.predict(X_test)

print(confusion_matrix(y_test, y_pred))
print(classification_report(y_test, y_pred))
print(accuracy_score(y_test, y_pred))

sample_text = "Just after midnight, a snowdrift stops the Orient Express in its tracks as it travels through the " \
              "mountainous Balkans. The luxurious train is surprisingly full for the time of the year but, " \
              "by the morning, it is one passenger fewer. An American tycoon lies dead in his compartment, " \
              "stabbed a dozen times, his door locked from the inside. One of the passengers is none other than " \
              "detective Hercule Poirot. On vacation. Isolated and with a killer in their midst, Poirot must identify " \
              "the murderer—in case he or she decides to strike again "

pred = classifier.predict([prepare_document(sample_text)])
print(f"Predicted: {pred[0]}")

pass
