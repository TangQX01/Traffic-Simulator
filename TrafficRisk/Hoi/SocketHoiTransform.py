# -*- coding: utf-8 -*-
# @Time : 2023/5/18 14:05
# @Author : qingxian
# @File : SocketHoiTransform.py
# @Software: PyCharm
import test_on_images as TI
import socket
import threading
from icecream import ic     #icecream为调试的包
import json
import os
import cv2

issendtime = False
dataset = None
upt = None
actions = None
image_save_path = ""
send_message = []

#  ****************************************************************************************************
#  网络通信
count = 0

def test_v():
    img = cv2.imread(r"E:\1-Project\riding-violation\HOI\HoiTransformer\log\Camera_Mid_20235154137.jpg")
    cv2.namedWindow("HOIDetect_img", 0)  # 0可调大小，注意：窗口名必须imshow里面的一窗口名一直
    # cv2.resizeWindow("capture_img", 384, 216)  # 设置长和宽
    # cv2.resizeWindow("HOIDetect_img", 576, 324)  # 设置长和宽
    cv2.resizeWindow("HOIDetect_img", 1152, 648)  # 设置长和宽
    cv2.imshow('HOIDetect_img', img)  # opencv显示
    cv2.waitKey(0)

def visualise_entire_image(rec_message={}):
    """Visualise bounding box pairs in the whole image by classes"""
    # Rescale the boxes to original image size
    image_name = str(rec_message["camera_name"]) + "_" + str(rec_message["capture_time"]) + '.jpg'
    image_save_path = os.path.join(r"E:\Project\riding-violation\HOI\upt\run\jpg\my_test",
                                   str(rec_message["camera_name"]), image_name)
    image_judge = {}
    image_judge["type_violation"] = None                    #违法类型
    image_judge["image_savepath"] = image_save_path         #识别后的图片存储位置
    image_judge["position_x"] = rec_message["position_x"]   #违法在unity中的x坐标
    image_judge["position_z"] = rec_message["position_z"]   #违法在unity中的z坐标

    # print("begin visualise detected human-object paires")
    # Visualise detected human-object pairs with attached scores
    is_violation = False

    # #在opencv中展示识别结果
    # cv2.namedWindow("HOIDetect_img", 0)  # 0可调大小，注意：窗口名必须imshow里面的一窗口名一直
    # # cv2.resizeWindow("capture_img", 384, 216)  # 设置长和宽
    # cv2.resizeWindow("HOIDetect_img", 576, 324)  # 设置长和宽
    # cv2.imshow('HOIDetect_img', img)  # opencv显示
    # cv2.waitKey(1)
    img = cv2.imread(os.path.join(r"E:\1-Project\riding-violation\HOI\HoiTransformer\log",image_name))
    print(os.path.join(r"E:\1-Project\riding-violation\HOI\HoiTransformer\log",image_name))
    cv2.namedWindow("HOIDetect_img", 0)  # 0可调大小，注意：窗口名必须imshow里面的一窗口名一直
    # cv2.resizeWindow("capture_img", 384, 216)  # 设置长和宽
    # cv2.resizeWindow("HOIDetect_img", 576, 324)  # 设置长和宽
    cv2.resizeWindow("HOIDetect_img", 1536, 864)  # 设置长和宽
    cv2.imshow('HOIDetect_img', img)  # opencv显示
    cv2.waitKey(0)

    # 违法类型
    if (is_violation):
        image_judge["type_violation"] = "摩托车骑手不带头盔"
    else:
        image_judge["type_violation"] = "未发现违法行为"

    return image_judge

#从文件的名字获取违法的经纬度, 图片的命名格式为camera.name+camera.position.x+camera.position.y+nowtime.jpg
def getCoordFromName(image_name):
    #第一步有可能输入的image_name是路径
    image_name = image_name.split("\\")[-1]
    name, posX, posY, nowtime = image_name.split("+")
    nowtime = nowtime.split(".")[0]
    # ic() ,ic(name), ic(posX), ic(posY), ic(nowtime)
    return name, posX, posY, nowtime

def DetectPic(rec_message):
    global issendtime, dataset, upt, actions, image_save_path, send_message

    img_list = [rec_message["save_path"]]
    TI.run_on_images(args=args, img_path_list=img_list)

    image_judge = visualise_entire_image(rec_message)
    if (image_judge):
        issendtime = True
        print(image_judge)
        # ic(image_judge)
        send_message.append(image_judge)

def client_sent(sock):
    global issendtime, send_message
    message_sent = dict()
    message_sent["file_name"] = r"my_code\jpg\test_{}.jpg".format(str(count).zfill(4))
    while True:
        if issendtime:
            # sock.send(image_save_path.encode('UTF-8'))
            for _send_message in send_message:
                sock.send((str(_send_message)+"end_message").encode("UTF-8"))
            send_message.clear()
            ic("已发送数据")
            issendtime = False

def client_recv(sock):
    while True:
        global instring, count, issendtime
        instring = sock.recv(1024000)  # 接收数据
        if instring != '':
            count += 1
            # print('收到数据如下：' + instring.decode('UTF-8'))
            rec_message_all = instring.decode("UTF-8")
            rec_message_list =  rec_message_all.split("end_message")[:-1]
            for rec_message in rec_message_list:
                #需要转换成字典
                rec_message = json.loads(rec_message)
                print(rec_message)
                DetectPic(rec_message)
            instring = ''

#连接socket，要打开unity
def connect_socket():
    clf = socket.socket()
    clf.connect(('127.0.0.1', 6666))
    print('服务器已连接')
    th_send = threading.Thread(target=client_sent, args=(clf,))  # 发送消息的线程
    th_send.start()
    th_recv = threading.Thread(target=client_recv, args=(clf,))  # 接受消息线程
    th_recv.start()
    th_send.join()
    th_recv.join()

def main():
    # 连接socket
    while True:
        try:
            connect_socket()
            break
        except:
            ic("已尝试重新连接")

if __name__ == "__main__":
    # test_v()
    parser = TI.get_args_parser()
    args = parser.parse_args()
    main()