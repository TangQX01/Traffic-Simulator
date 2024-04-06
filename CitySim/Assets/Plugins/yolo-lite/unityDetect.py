import cv2
import numpy as np
from mss import mss

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


while True:
    # 截屏并保存到指定目录
    # sct_img = mss().grab(bounding_box)
    # scr_img = np.array(sct_img)
    # cv2.imwrite(os.path.join(ROOT / 'data/screenShot', 'test.jpg'), scr_img)

    # 推理
    detect.run(weights=str(ROOT / 'weights/best.pt'),  # model.pt path(s) 权重
               # data=ROOT / 'data/King.yaml',  # dataset.yaml path 模型
               source=str(ROOT / 'data/screenShot/test.jpg'),  # 截图文件路径
               exist_ok=True,  # existing project/name ok, do not increment # 覆盖推理结果
               )

    # 读取推理结果并展示
    scr_img = cv2.imread(str(ROOT / 'runs/detect/exp/test.jpg'), flags=1)
    scr_img = np.array(scr_img)
    scr_img = cv2.resize(scr_img, (800, 400))
    cv2.imshow("Screen Realtime", scr_img)
    # 按下q则终止程序
    if (cv2.waitKey(1) & 0xFF) == ord('q'):
        cv2.destroyAllWindows()
        break