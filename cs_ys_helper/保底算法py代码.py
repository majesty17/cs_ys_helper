## -*- coding:utf-8 -*-
import random
import datetime, time

rate_5 = 0.006
rate_4 = 0.051

ct = 0  # 总次数
ct_4 = 0  # 中4的次数
ct_5 = 0  # 中5的次数

no_4_ct = 0  # 连续多少次没中4、5星
no_5_ct = 0  # 连续多少次没中5

random.seed(time.time())
for i in range(10000000):
    ran = random.random()
    ct += 1
    if no_4_ct >= 9 and no_5_ct >= 89:  # 同时保底,保5，同时重置
        no_4_ct = 0
        no_5_ct = 0
        ct_5 += 1
    elif no_4_ct >= 9:  # 仅小保底，从45里随机
        ran = random.random()
        if ran < rate_5:
            ct_5 += 1
            no_4_ct = 0
            no_5_ct = 0
        else:  # 不中5的话，就是4
            ct_4 += 1
            no_4_ct = 0
            no_5_ct += 1
    elif no_5_ct >= 89:  # 仅大保底，都重置
        ct_5 += 1
        no_4_ct = 0
        no_5_ct = 0
    else:  # 并没有触发保底，随啥是啥
        ran = random.random()
        # print(ran)
        if ran < rate_5:
            no_4_ct = 0
            no_5_ct = 0
            ct_5 += 1
        elif ran >= rate_5 and ran < rate_5 + rate_4:
            no_4_ct = 0
            ct_4 += 1
            no_5_ct += 1
        else:
            no_4_ct += 1
            no_5_ct += 1

print(100.0 * ct_5 / ct, 100.0 * ct_4 / ct)
