import cv2
import numpy as np
from mss import mss
import argparse
import detect

import os
import sys
from pathlib import Path

# # 截图区域
# bounding_box = {'top': 0, 'left': 0, 'width': 1960, 'height': 1080}
# # 项目根路径
FILE = Path(__file__).resolve()
ROOT = FILE.parents[0]  # YOLOv5 root directory
if str(ROOT) not in sys.path:
    sys.path.append(str(ROOT))  # add ROOT to PATH


if __name__ == '__main__':
    # parser = argparse.ArgumentParser()
    #
    # parser.add_argument('--weights', nargs='+', type=str, default=r'E:\Project\Unity\citysimulator\Assets\My\yolo-lite\weights\best.pt', help='model.pt path(s)')
    # parser.add_argument('--source', type=str, default=r'E:\Project\Unity\citysimulator\Assets\My\CameraCapture\cameraCapture.jpg', help='source')  # file/folder, 0 for webcam

    while True:
        detect.run(weights=r'E:\Project\Unity\citysimulator\Assets\My\yolo-lite\weights\best.pt',  # model.pt path(s) 权重
                   # data=ROOT / 'data/King.yaml',  # dataset.yaml path 模型
                   source=r'E:\Project\Unity\citysimulator\Assets\My\CameraCapture\cameraCapture.jpg',  # 截图文件路径
                   # source=r'E:\Project\Unity\citysimulator\Assets\My\CameraCapture',  # 截图文件路径
                   exist_ok=True,  # existing project/name ok, do not increment # 覆盖推理结果
                   )

    # # 读取推理结果并展示
    # scr_img = cv2.imread(str(ROOT / 'runs/detect/exp/cameraCapture.jpg'), flags=1)
    # scr_img = np.array(scr_img)
    # scr_img = cv2.resize(scr_img, (800, 400))
    # cv2.imshow("Screen Realtime", scr_img)