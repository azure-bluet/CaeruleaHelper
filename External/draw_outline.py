# External tool note: script-compatible

from PIL import Image, ImageFile
from sys import argv

# Usage: draw_outline.py <input> <output>

def apply (img: ImageFile.ImageFile) -> ImageFile.ImageFile:
    img = img.convert ("RGBA")
    size = img.size
    canvas = Image.new ("RGBA", size)

    def valid (x, y):
        return x >= 0 and x < size [0] and\
            y >= 0 and y < size [1] and\
            img.getpixel ((x, y)) [3] > 0

    for i in range (size [0]):
        for j in range (size [1]):
            if (valid (i, j+1) or valid (i, j-1) or valid (i+1, j) or valid (i-1, j))\
                and not valid (i, j):
                canvas.putpixel ((i, j), (255, 255, 255, 255))

    return canvas

if __name__ == '__main__':
    inf = Image.open (argv [1])
    outf = apply (inf)
    outf.save (argv [2])
