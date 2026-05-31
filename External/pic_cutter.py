from PIL import Image
from math import gcd
from sys import argv

def split_img (pth):
    img = Image.open (pth)
    width, height = img.size
    g = gcd (*img.size)
    count = 0
    for x in range (0, width, g):
        for y in range (0, height, g):
            count += 1
            img.crop ((x, y, x+g, y+g)) .save (f"output_{count}.png")

if __name__ == "__main__": split_img (argv [1])
