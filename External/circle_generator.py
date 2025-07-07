from PIL import Image
from sys import argv
from math import sin, cos, pi, atan, sqrt
from random import uniform

# hex to color
def h2c (s):
    hx = int (s, 16)
    if len (s) == 6: return (hx >> 16), (hx >> 8) & 255, hx & 255, 255
    else: return (hx >> 24), (hx >> 16) & 255, (hx >> 8) & 255, hx & 255

# parameters
def parse_params (name):
    try:
        idx = argv.index ('--' + name)
        return argv [idx+1]
    except (ValueError, IndexError):
        return input (f'{name}: ')
width = int (parse_params ('width'))
height = int (parse_params ('height'))
radius = float (parse_params ('radius'))
cx = int (parse_params ('centerX'))
cy = int (parse_params ('centerY'))
clr = h2c (parse_params ('color'))

# create function
def create (func, mul, con, pm):
    return lambda theta: mul * func (pm * theta + con)

l_func = []
for i in range (8): l_func.extend ([
    create (sin, uniform (-1/8, 1/8), uniform (0, 2*pi), i+1),
    create (cos, uniform (-1/8, 1/8), uniform (0, 2*pi), i+1)
])

img = Image.new ("RGBA", (width, height), (0, 0, 0, 0))
# fill
for i in range (width):
    for j in range (height):
        dx = i - cx
        dy = j - cy
        angle = atan (dx / dy) if dy else pi/2
        angle += pi/2 if dx > 0 else -pi/2
        rd = radius
        for f in l_func: rd += f (angle) * radius / 6
        if sqrt (dx*dx + dy*dy) <= rd: img.putpixel ((i, j), clr)
img.save (parse_params ('output'))
