import numpy as np
import matplotlib.pyplot as plt
import matplotlib.image
import cv2

def GenerateGradientImage(list_of_colors,size):

    image = np.zeros(size,dtype=np.uint8)
    parts = len(list_of_colors) -1
    print("N of Parts : ",parts)
    part_size = size[0] / parts
    print("Part Size : ",part_size)
    ranges = (np.array(range(parts+1)) * part_size).astype(int)
    print("Ranges : ",ranges)

    for i in range(0,len(ranges)-1):
        print(f"Treating Range : {ranges[i]} to {ranges[i+1]}")
        start = ranges[i]
        end = ranges[i+1]
        print(f"Starting part ")

        for j in range(start,end):

            coef = (j - end)/(start-end)
            color = list_of_colors[i] * coef + list_of_colors[i+1] * (1-coef)
            #print(f"Color = {color}")
            image[j,:] = color

    return np.flip(image,0)


dark_blue = np.array((60,0,80))
dark_blue2 = np.array((70,10,90))
dark_blue3 = np.array((80,20,100))
light_blue = np.array((100,20,120))
dark_red = np.array((200,50,80))
yellow = np.array((220,220,0))
white = np.array((255,255,245))
im = GenerateGradientImage([dark_blue,dark_blue2,dark_blue3,light_blue,dark_red,yellow,white,white],(100,100,3))
im = cv2.blur(im,(8,8))
plt.imshow(im, interpolation='nearest')
plt.show()


matplotlib.image.imsave('UnityCours\My Project\Assets\Thermal\GradText.png', im)