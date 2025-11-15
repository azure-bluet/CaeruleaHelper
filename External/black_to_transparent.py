# External tool note: script-compatible

from PIL import Image, ImageFile
from sys import argv

def apply (img: ImageFile.ImageFile) -> ImageFile.ImageFile:
    img = img.convert ("RGBA")
    size = img.size

    def isblack (pixel):
        return pixel [0] == 0 and pixel [1] == 0 and pixel [2] == 0

    for i in range (size [0]):
        for j in range (size [1]):
            if isblack (img.getpixel ((i, j))):
                img.putpixel ((i, j), (0, 0, 0, 0))

    return img

if __name__ == '__main__':
    inf = Image.open (argv [1])
    outf = apply (inf)
    outf.save (argv [2])
