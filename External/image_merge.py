from PIL import Image
from os.path import exists

img = Image.new ("RGBA", (320, 180))
# poorly designed
i = 1
while exists (f'{i}.png'): i += 1
i -= 1
for j in range (i, 0, -1):
    im = Image.open (f'{j}.png')
    for x in range (320):
        for y in range (180):
            px = im.getpixel ((x, y))
            if px [3]: img.putpixel ((x, y), px)
img.save ('merged.png')
