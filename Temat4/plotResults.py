import matplotlib.pyplot as plt
import numpy as np
from turtle import color
import pandas as pd


df = pd.read_csv("./results/training_model.csv")
x = df['model'].to_numpy()
best = df['best_score'].to_numpy()
worst = df['worst_score'].to_numpy()

x_axis = np.arange(len(x))

plt.bar(x_axis, best, width=0.4, label='Best', color="g")
plt.bar(x_axis, worst, width=0.4, label='Worst', color="r")

plt.xticks(x_axis, x, rotation=20)
plt.legend()
plt.show()

# ========================

df = pd.read_csv("./results/alpha.csv")
x = df['model'].to_numpy()
best = df['best_score'].to_numpy()
worst = df['worst_score'].to_numpy()

x_axis = np.arange(len(x))

plt.plot(x_axis, best, label='Best', color="g")
plt.plot(x_axis, worst, label='Worst', color="r")

plt.xticks(x_axis, x, rotation=20)
plt.legend()
plt.show()

# ========================

df = pd.read_csv("./results/kernels.csv")
x = df['model'].to_numpy()
best = df['best_score'].to_numpy()
worst = df['worst_score'].to_numpy()

x_axis = np.arange(len(x))

plt.bar(x_axis, best, label='Best', color="g")
plt.bar(x_axis, worst, label='Worst', color="r")

plt.xticks(x_axis, x, rotation=20)
plt.legend()
plt.show()

# ======================

df = pd.read_csv("./results/features_count.csv")
x = df['fatures_after_selection'].to_numpy()
best = df['best_score'].to_numpy()
worst = df['worst_score'].to_numpy()

x_axis = np.arange(len(x))

plt.plot(x_axis, best, label='Best', color="g")
plt.plot(x_axis, worst, label='Worst', color="r")

plt.xticks(x_axis, x, rotation=20)
plt.legend()
plt.show()
