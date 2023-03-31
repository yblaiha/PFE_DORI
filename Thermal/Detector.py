import numpy as np
import matplotlib.pyplot as plt
import matplotlib.image
import cv2
import os


img = cv2.imread('UnityCours/My Project/Assets/Thermal/Temp2.png',1)
grad = cv2.imread('UnityCours/My Project/Assets/Thermal/ThermalGradient1.png', 1)
img = cv2.cvtColor(img,cv2.COLOR_BGR2RGB)
grad = cv2.cvtColor(grad,cv2.COLOR_BGR2RGB)
grad = cv2.flip(grad, 0)

sensitivity = 20
ambient = 20
threshold = 500

f = lambda x : max(0,min(1,(x-ambient)/sensitivity))
bound_min = f(28)
bound_max = f(42)

tex_size = grad.shape[0] - 1


final = np.zeros(img.shape[:2],dtype = 'uint8')
def get_mask(b_min):
    color_min = grad[b_min,0]

    color_bound_min = np.array(0.75 * color_min)
    color_bound_max = np.array(1.1 * color_min)

    mask = cv2.inRange(img, color_bound_min, color_bound_max)
    return mask

for k in range(int(tex_size * bound_min),int(tex_size * bound_max)):
    mask = get_mask(k)
    final = cv2.bitwise_or(final,mask)

kernel = np.ones((5, 5), np.uint8)
final_dil = cv2.dilate(final, kernel, iterations=3)

output = cv2.connectedComponentsWithStats(final_dil, 8, cv2.CV_32S)

for i in range(1,output[0]):
    if output[2][i][4] < threshold:
        continue
    bounds = output[2][i]
    pt1 = bounds[0:2]
    pt2 = np.array([pt1[0]+bounds[2],pt1[1]+bounds[3]])
    cv2.rectangle(img,pt1,pt2,(255,0,0),2)
    print(output[3][i])


plt.imshow(final_dil, interpolation='nearest')
plt.imshow(img, interpolation='nearest')
plt.show()

