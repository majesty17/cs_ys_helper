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


# 通过之前连续不中的次数x，推测出下一次的概率
def get_new_rate(x):
    if (x >= 72):
        return (1.0 - 0.006) / (89 - 72) * (x - 72) + 0.006
    else:
        return 0.006


def get_new_rate4(x):
    if (x >= 6):
        return (1.0 - 0.051) / (9.0 - 6.0) * (x - 6.0) + 0.051
    else:
        return 0.051


for i in range(1000000):
    ran = random.random()
    ct += 1
    if no_5_ct >= 89:  # 保5，同时重置
        no_4_ct = 0
        no_5_ct = 0
        ct_5 += 1
    elif no_4_ct >= 9:  # 仅小保底，从45里随机
        ran = random.random()
        if ran < get_new_rate(no_5_ct):
            ct_5 += 1
            no_4_ct = 0
            no_5_ct = 0
        else:  # 不中5的话，就是4
            ct_4 += 1
            no_4_ct = 0
            no_5_ct += 1

    else:  # 并没有触发保底，随啥是啥
        ran = random.random()
        # print(ran)
        if ran < get_new_rate(no_5_ct):
            no_4_ct = 0
            no_5_ct = 0
            ct_5 += 1
        elif ran >= get_new_rate(no_5_ct) and ran < get_new_rate(no_5_ct) + get_new_rate4(no_4_ct):
            no_4_ct = 0
            ct_4 += 1
            no_5_ct += 1
        else:
            no_4_ct += 1
            no_5_ct += 1

print(100.0 * ct_5 / ct, 100.0 * ct_4 / ct)
