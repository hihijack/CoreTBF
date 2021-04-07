import sqlite3
import csv
import os

csvs = [x for x in os.listdir('.') if os.path.isfile(x) and os.path.splitext(x)[1]=='.csv']

#数据库连接
DB_NAME = 'coretbf.db'
conn = sqlite3.connect(DB_NAME)
cur = conn.cursor()

print('数据库%s连接成功' % DB_NAME)

for i,csvFile in enumerate(csvs):
    print('开始导入', csvFile)
    ROW_COUNT = 0
    ROW_INDEX = -1
    with open(csvFile, 'r', encoding = 'gbk') as f:
        reader = csv.reader(f)
        tableName = os.path.splitext(csvFile)[0]
        #删除旧的数据
        cur.execute("DELETE from %s" % (tableName))
        for field in reader:
            #取第一个元素，如果以#开头，认为是注释
            headVal = field[0]
            if headVal.startswith('#'):
                continue
            ROW_INDEX += 1
            #导入到数据库
            if ROW_INDEX > 0:
                field = ["'%s'" % x for x in field]
                cur.execute("INSERT INTO %s VALUES (%s);" % (tableName, ",".join(field)))
                ROW_COUNT += 1
    print('%s 导入完成,数据%d条' % (csvFile, ROW_COUNT))
conn.commit()
conn.close()

print('执行完成,数据库%s关闭' % DB_NAME)
input('按任意键退出')
