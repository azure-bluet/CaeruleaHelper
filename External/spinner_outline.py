from os import listdir
from PIL import Image, ImageFile
from sys import argv

import black_to_transparent as blacktrans
import draw_outline

even = lambda x: x if x%2==0 else x-1

def apply (img: ImageFile.ImageFile, size_multiplier: int = 12) -> ImageFile.ImageFile:
    size = img.size
    size = (size [0] // size_multiplier, size [1] // size_multiplier)
    size = (even (size [0]), even (size [1]))
    canvas = Image.new ("RGBA", size)
    for i in range (size [0]):
        for j in range (size [1]):
            canvas.putpixel ((i, j), img.getpixel ((i*size_multiplier, j*size_multiplier)))
    img = blacktrans.apply (canvas)
    img = draw_outline.apply (img)
    return img

if __name__ == '__main__':
    path = argv [1]
    sm = 12
    if len (argv) > 2: sm = int (argv [2])
    for name in listdir (path):
        if name.endswith ('.png'):
            prefix = name [:-4]
            img = Image.open (path + name)
            img = apply (img, sm)
            img.save (path + prefix + '_outlined.png')
