import xgboost as xgb
import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, classification_report
import matplotlib.pyplot as plt
import seaborn as sns
from sklearn.preprocessing import LabelEncoder
from sklearn.preprocessing import PowerTransformer
from sklearn.preprocessing import RobustScaler

class XGBoost:
    def __init__(self, filepath):
        self.zip_file_path = filepath
        self.extract_to = r'D:\Users\praktik\Downloads\train_data'
    TARGET = "erly_pnsn_flg"
    ID = 'accnt_id'

    base_data = pd.read_csv('d:/Users/praktik/Downloads/test_data/test_data/cntrbtrs_clnts_ops_tst.csv', encoding='windows-1251', sep=';', on_bad_lines='skip')
    transactions = pd.read_csv('d:/Users/praktik/Downloads/test_data/test_data/trnsctns_ops_tst.csv', encoding='windows-1251', sep=';', on_bad_lines='skip')

    data = pd.merge(base_data, transactions, on='accnt_id', how='inner')

    binary_columns = ['phn', 'email', 'lk']

    for col in binary_columns:
        data[col] = data[col].replace({'нет': 0, 'да': 1})
    data.isnull().sum()

    numeric_cols = data.select_dtypes(include=['int', 'float']).columns
    data['okato'] = data['okato'].fillna(0.0e+10)
    data['rgn'] = data['rgn'].fillna('НЕТ ДАННЫХ')
    useless_columns = ['clnt_id', 'dstrct', 'city', 'sttlmnt', 'prvs_npf']
    data.drop(columns=useless_columns, inplace=True)

    outlier_features = ['cprtn_prd_d', 'brth_yr', 'prsnt_age', 'sum']

    def transform_outliers(df, columns):
        for col in columns:
            df[col] = np.where(
                df[col] > 0,
                np.sqrt(np.log1p(df[col])),
                df[col]
            )
        pt = PowerTransformer(method='yeo-johnson')
        transformed_data = pt.fit_transform(df[columns])

        df[columns] = transformed_data

        return df

    data = transform_outliers(data, outlier_features)


    cat_cols = ['gndr', 'accnt_bgn_date', 'accnt_status', 'brth_plc',
           'addrss_type', 'rgn', 'assgn_npo', 'assgn_ops', 'sum_type', 'cmmnt',
           'oprtn_date', 'pstl_code']




    def label_encode(df, cols):
        label_encoder = LabelEncoder()
        for col in cols:
            df[col] = label_encoder.fit_transform(df[col].astype(str))
        return df

    data = label_encode(data, cat_cols)


    data_no_id = data.drop('accnt_id', axis=1)



    ID = 'accnt_id'

    def apply_scaler(df, column):
        scaler = RobustScaler()
        columns = [col for col in df.columns if col != column]
        scaled_data = scaler.fit_transform(df[columns])
        df_scaled = pd.DataFrame(scaled_data, columns=columns)
        df_scaled[column] = df[column].reset_index(drop=True)

        return df_scaled

    data = apply_scaler(data, ID)


    cols = ['slctn_nmbr', 'gndr', 'brth_yr', 'prsnt_age',
           'accnt_bgn_date', 'cprtn_prd_d', 'erly_pnsn_flg', 'accnt_status',
           'pnsn_age', 'brth_plc', 'addrss_type', 'rgn', 'pstl_code', 'okato',
           'phn', 'email', 'lk', 'assgn_npo', 'assgn_ops', 'mvmnt_type',
           'sum_type', 'cmmnt', 'sum', 'oprtn_date']


    import joblib
    import os

    model_directory = r'd:\Users\praktik\Downloads\train_data\train_data\model'
    model_filename = 'XGBoost.joblib'

    if not os.path.exists(model_directory):
        os.makedirs(model_directory)

    loaded_model = joblib.load(os.path.join(model_directory, model_filename))

    predictions = loaded_model.predict(data_no_id)

    results = pd.DataFrame({
            'accnt_id': data['accnt_id'].reset_index(drop=True),
            'erly_pnsn_flg': predictions
        })

    results.to_json('d:/Users/praktik/Downloads/predictionsfixedwin.csv', index=False, encoding='windows-1251')




